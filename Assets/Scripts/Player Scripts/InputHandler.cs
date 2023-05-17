using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
  public float horizontal;
  public float vertical;
  public float mouseX;
  public float mouseY;
  public bool attackInput, aimInput, sprintInput, crouchInput, jumpInput;

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
      inputActions.PlayerActions.Attack.canceled += i => attackInput = false;
      inputActions.PlayerActions.Aim.performed += i => aimInput = true;
      inputActions.PlayerActions.Aim.canceled += i => aimInput = false;
      inputActions.PlayerActions.Crouch.performed += i => crouchInput = true;
      inputActions.PlayerActions.Sprint.performed += i => sprintInput = true;
      inputActions.PlayerActions.Sprint.canceled += i => sprintInput = false;
      inputActions.PlayerActions.Jump.performed += i => jumpInput = true;
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
