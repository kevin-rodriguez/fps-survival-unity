using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KR
{
  public class WeaponPickup : Interactable
  {

    public GameObject weaponPrefab;

    protected override void Interact(PlayerInteract playerInteract)
    {
      base.Interact(playerInteract);

      print("Picked up " + weaponPrefab.name);

      playerInteract.PickupWeapon(weaponPrefab);

      //Destroy(gameObject);

    }
  }
}