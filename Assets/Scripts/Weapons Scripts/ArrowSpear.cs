using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpear : MonoBehaviour
{
  public event Action<Transform> onHit;
  private Rigidbody objectRigidbody;
  public float projectileSpeed = 30f;
  public float deactivateTimer = 5f;
  public float damage = 15f;



  void Awake()
  {
    objectRigidbody = GetComponent<Rigidbody>();
  }
  void Start()
  {
    Invoke("DeactivateGameObject", deactivateTimer);
  }

  public void Launch(Camera mainCamera)
  {
    objectRigidbody.velocity = mainCamera.transform.forward * projectileSpeed;

    // Point projectile to camera's view
    transform.LookAt(transform.position + objectRigidbody.velocity);
  }

  void DeactivateGameObject()
  {
    if (gameObject.activeInHierarchy)
    {
      //TODO: Implement object pooling
      Destroy(gameObject);
    }
  }


  private void OnTriggerEnter(Collider collider)
  {
    //objectRigidbody.isKinematic = true; //TODO: Make it stick

    if (onHit != null)
    {
      print("Hit with projectile: " + collider.transform.name);
      onHit.Invoke(collider.transform);
    }
  }
}
