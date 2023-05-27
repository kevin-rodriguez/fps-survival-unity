using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class DecalController : MonoBehaviour
  {
    private const float MIN_DECAL_SIZE = 0.10f;
    private const float MAX_DECAL_SIZE = 0.18f;

    [System.Serializable]
    public class TagDecalPair
    {
      public string tag;
      public GameObject decalPrefab;
    }

    [SerializeField]
    private List<TagDecalPair> decalPrefabs = new List<TagDecalPair>();

    [SerializeField]
    private int maxConcurrentDecals = 10;

    private Dictionary<string, Queue<GameObject>> decalsInPool;
    private Dictionary<string, Queue<GameObject>> decalsActiveInWorld;


    private void Awake()
    {
      InitializeDecals();
    }

    private void InitializeDecals()
    {
      decalsInPool = new Dictionary<string, Queue<GameObject>>();
      decalsActiveInWorld = new Dictionary<string, Queue<GameObject>>();

      foreach (var pair in decalPrefabs)
      {
        string tag = pair.tag;
        GameObject prefab = pair.decalPrefab;

        decalsInPool[tag] = new Queue<GameObject>();
        decalsActiveInWorld[tag] = new Queue<GameObject>();

        for (int i = 0; i < maxConcurrentDecals; i++)
        {
          InstantiateDecal(tag, prefab);
        }
      }
    }

    private void InstantiateDecal(string tag, GameObject prefab)
    {
      var spawned = Instantiate(prefab);
      spawned.transform.SetParent(transform);

      decalsInPool[tag].Enqueue(spawned);
      spawned.SetActive(false);
    }


    private float GetRandomDecalSize()
    {
      return Random.Range(MIN_DECAL_SIZE, MAX_DECAL_SIZE);
    }

    private Vector3 GetRandomDecalScale()
    {
      float randomScaleX = GetRandomDecalSize();
      float randomScaleY = GetRandomDecalSize();

      return new Vector3(randomScaleX, randomScaleY, MIN_DECAL_SIZE);
    }

    public void SpawnDecal(RaycastHit hit, string tag, float sizeMultiplier = 1f)
    {
      if (decalPrefabs.Exists(pair => pair.tag == tag))
      {
        GameObject decalPrefab = decalPrefabs.Find(pair => pair.tag == tag).decalPrefab;
        GameObject decal = GetNextAvailableDecal(tag);
        if (decal != null)
        {
          AudioSource audioSource = decal.GetComponent<AudioSource>();
          ParticleSystem[] particleSystems = decal.GetComponentsInChildren<ParticleSystem>();

          decal.transform.position = hit.point + hit.normal * 0.1f;
          decal.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
          decal.transform.localScale = GetRandomDecalScale() * sizeMultiplier;

          foreach (ParticleSystem ps in particleSystems)
          {
            ps.Play();
          }

          decal.SetActive(true);

          if (audioSource)
          {
            audioSource.Play();
          }

          decalsActiveInWorld[tag].Enqueue(decal);
        }
      }
      else
      {
        Debug.LogWarning("No decal prefab found for tag: " + tag);
      }
    }

    private GameObject GetNextAvailableDecal(string tag)
    {
      if (decalsInPool.ContainsKey(tag) && decalsInPool[tag].Count > 0)
      {
        return decalsInPool[tag].Dequeue();
      }

      if (decalsActiveInWorld.ContainsKey(tag) && decalsActiveInWorld[tag].Count > 0)
      {
        var oldestActiveDecal = decalsActiveInWorld[tag].Dequeue();
        return oldestActiveDecal;
      }

      return null;
    }



    // #if UNITY_EDITOR
    //     private void Update()
    //     {
    //       if (transform.childCount < maxConcurrentDecals)
    //         foreach (var pair in decalPrefabs)
    //         {
    //           string tag = pair.tag;
    //           GameObject prefab = pair.decalPrefab;

    //           decalsInPool[tag] = new Queue<GameObject>();
    //           decalsActiveInWorld[tag] = new Queue<GameObject>();

    //           for (int i = 0; i < maxConcurrentDecals; i++)
    //           {
    //             InstantiateDecal(tag, prefab);
    //           }
    //         }
    //       else if (ShoudlRemoveDecal(tag))
    //         DestroyExtraDecal();
    //     }

    //     private bool ShoudlRemoveDecal()
    //     {
    //       return transform.childCount > maxConcurrentDecals;
    //     }

    //     private void DestroyExtraDecal()
    //     {
    //       if (decalsInPool.Count > 0)
    //         Destroy(decalsInPool.Dequeue());
    //       else if (ShoudlRemoveDecal() && decalsActiveInWorld.Count > 0)
    //         Destroy(decalsActiveInWorld.Dequeue());
    //     }

    // #endif


  }
}