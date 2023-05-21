using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class DecalPlacer : MonoBehaviour
  {
    [SerializeField]
    private GameObject decalPrefab;

    public void SpawnDecal(RaycastHit hitInfo)
    {
      var decal = Instantiate(decalPrefab);

      decal.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z - 0.01f);
      decal.transform.forward = hitInfo.normal * -1f;
      decal.transform.SetParent(hitInfo.transform);
    }
  }
}