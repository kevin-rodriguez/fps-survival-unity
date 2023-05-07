using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpear : MonoBehaviour
{

  private Rigidbody objectRigidbody;
  public float projectileSpeed = 30f;
  public float deactivateTimer = 3f;
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
      gameObject.SetActive(false);
    }
  }

  void OnTriggerEnter(Collider target)
  {
    //
  }
}
