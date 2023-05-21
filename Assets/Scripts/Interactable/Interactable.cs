using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{

  public abstract class Interactable : MonoBehaviour
  {
    public string promptMessage;

    public void BaseInteract()
    {
      Interact();
    }

    protected virtual void Interact()
    {
      // Overwrite it
    }
  }
}
