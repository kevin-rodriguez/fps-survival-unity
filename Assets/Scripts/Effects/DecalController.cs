using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class DecalController : MonoBehaviour
  {
    private const float MIN_DECAL_SIZE = 0.10f;
    private const float MAX_DECAL_SIZE = 0.18f;
    [SerializeField]
    private GameObject bulletHoleDecalPrefab;

    [SerializeField]
    private int maxConcurrentDecals = 10;

    private Queue<GameObject> decalsInPool;
    private Queue<GameObject> decalsActiveInWorld;

    private void Awake()
    {
      InitializeDecals();
    }

    private void InitializeDecals()
    {
      decalsInPool = new Queue<GameObject>();
      decalsActiveInWorld = new Queue<GameObject>();

      for (int i = 0; i < maxConcurrentDecals; i++)
      {
        InstantiateDecal();
      }
    }

    private void InstantiateDecal()
    {
      var spawned = GameObject.Instantiate(bulletHoleDecalPrefab);
      spawned.transform.SetParent(this.transform);

      decalsInPool.Enqueue(spawned);
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

    public void SpawnDecal(RaycastHit hit)
    {
      GameObject decal = GetNextAvailableDecal();
      if (decal != null)
      {
        decal.transform.position = hit.point + hit.normal * 0.1f;

        decal.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hit.normal);

        decal.transform.localScale = GetRandomDecalScale();

        ParticleSystem[] particleSystems = decal.GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particleSystems)
        {
          ps.Play();
        }

        decal.SetActive(true);

        decalsActiveInWorld.Enqueue(decal);
      }
    }

    private GameObject GetNextAvailableDecal()
    {
      if (decalsInPool.Count > 0)
        return decalsInPool.Dequeue();

      var oldestActiveDecal = decalsActiveInWorld.Dequeue();
      return oldestActiveDecal;
    }

#if UNITY_EDITOR

    private void Update()
    {
      if (transform.childCount < maxConcurrentDecals)
        InstantiateDecal();
      else if (ShoudlRemoveDecal())
        DestroyExtraDecal();
    }

    private bool ShoudlRemoveDecal()
    {
      return transform.childCount > maxConcurrentDecals;
    }

    private void DestroyExtraDecal()
    {
      if (decalsInPool.Count > 0)
        Destroy(decalsInPool.Dequeue());
      else if (ShoudlRemoveDecal() && decalsActiveInWorld.Count > 0)
        Destroy(decalsActiveInWorld.Dequeue());
    }

#endif
  }
}