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
    private PlayerManager playerManager;

    [SerializeField]
    private Transform weaponParent;

    void Start()
    {
      playerUI = GetComponent<PlayerUI>();
      inputHandler = GetComponent<InputHandler>();
      playerManager = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
      playerUI.ClearText();

      Ray ray = new Ray(camera.transform.position, camera.transform.forward);
      //Debug.DrawRay(ray.origin, ray.direction * distance);

      RaycastHit raycastHit;

      if (Physics.Raycast(ray, out raycastHit, distance, layerMask))
      {
        Interactable interactable = raycastHit.collider.GetComponent<Interactable>();

        playerUI.UpdateText(interactable.promptMessage);

        playerManager.isInteracting = true;

        if (inputHandler.interactInput)
        {
          interactable.BaseInteract(this);
        }
      }
      else
      {
        playerManager.isInteracting = false;
      }
    }

    public void PickupWeapon(GameObject weaponPrefab)
    {
      GameObject weaponInstance = Instantiate(weaponPrefab, weaponParent);

      playerManager.AddWeaponToManager(weaponInstance.GetComponent<WeaponHandler>());
      // Store the weapon instance in the player's inventory or equipped weapon reference
    }
  }
}
