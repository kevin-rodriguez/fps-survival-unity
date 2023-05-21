using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerInteract : MonoBehaviour
  {
    [SerializeField]
    private Camera camera;
    [SerializeField]
    private float distance = 3f;
    [SerializeField]
    private LayerMask layerMask;
    private PlayerUI playerUI;
    private InputHandler inputHandler;

    void Start()
    {
      playerUI = GetComponent<PlayerUI>();
      inputHandler = GetComponent<InputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
      playerUI.ClearText();

      Ray ray = new Ray(camera.transform.position, camera.transform.forward);
      Debug.DrawRay(ray.origin, ray.direction * distance);

      RaycastHit raycastHit;

      if (Physics.Raycast(ray, out raycastHit, distance, layerMask))
      {
        Interactable interactable = raycastHit.collider.GetComponent<Interactable>();

        playerUI.UpdateText(interactable.promptMessage);

        if (inputHandler.interactInput)
        {
          interactable.BaseInteract();
        }
      }
    }
  }
}
