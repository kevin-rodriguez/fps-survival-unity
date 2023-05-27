using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public abstract class Interactable : MonoBehaviour
  {
    public string promptMessage;

    public void BaseInteract(PlayerInteract playerInteract)
    {
      Interact(playerInteract);
    }

    protected virtual void Interact(PlayerInteract playerInteract)
    {
      // Overwrite it
    }
  }
}
