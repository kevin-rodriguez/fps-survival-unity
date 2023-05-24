using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
  public float horizontal;
  public float vertical;
  public float mouseX;
  public float mouseY;
  public bool attackInput, aimInput, sprintInput, crouchInput, jumpInput, pauseInput, interactInput, reloadInput;

  [Header("Flags")]
  public bool sprintFlag;

  InputActions inputActions;
  Vector2 movementInput;
  Vector2 cameraInput;

  public void OnEnable()
  {
    if (inputActions == null)
    {
      inputActions = new InputActions();
      inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
      inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
      inputActions.PlayerActions.Attack.performed += i => attackInput = true;
      inputActions.PlayerActions.Aim.performed += i => aimInput = true;
      inputActions.PlayerActions.Aim.canceled += i => aimInput = false;
      inputActions.PlayerActions.Crouch.performed += i => crouchInput = true;
      inputActions.PlayerActions.Sprint.performed += i => sprintInput = true;
      inputActions.PlayerActions.Sprint.canceled += i => sprintInput = false;
      inputActions.PlayerActions.Jump.performed += i => jumpInput = true;
      inputActions.PlayerActions.Interact.performed += i => interactInput = true;
      inputActions.PlayerActions.Reload.performed += i => reloadInput = true;
      inputActions.UI.Cancel.performed += i => pauseInput = true;
    }

    inputActions.Enable();
  }

  private void OnDisable()
  {
    inputActions.Disable();
  }

  public void TickInput(float delta)
  {
    MoveInput(delta);
  }

  private void MoveInput(float delta)
  {
    horizontal = movementInput.x;
    vertical = movementInput.y;
    mouseX = cameraInput.x;
    mouseY = cameraInput.y;
  }
}
