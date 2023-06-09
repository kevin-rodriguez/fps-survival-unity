using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class Detonator : Interactable
  {
    [SerializeField]
    private GameObject fireObject;
    private bool isActive;

    protected override void Interact(PlayerInteract playerInteract)
    {
      base.Interact(playerInteract);

      isActive = !isActive;
      fireObject.SetActive(isActive);
    }
  }
}