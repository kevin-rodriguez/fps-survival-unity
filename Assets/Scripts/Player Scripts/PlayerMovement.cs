using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
  CharacterController characterController;
  Vector3 moveDirection;
  InputHandler inputHandler;

  public float speed = 5f;
  private float gravity = 20f;
  public float jumpForce = 8f;
  private float verticalVelocity;

  private void Awake()
  {
    characterController = GetComponent<CharacterController>();
    inputHandler = GetComponent<InputHandler>();
  }

  public void MovePlayer()
  {
    moveDirection = new Vector3(inputHandler.horizontal, 0f, inputHandler.vertical);
    moveDirection = transform.TransformDirection(moveDirection);
    moveDirection *= speed * Time.deltaTime;

    ApplyGravity();

    characterController.Move(moveDirection);
  }

  bool isGrounded()
  {
    return characterController.isGrounded;
  }

  void ApplyGravity()
  {
    verticalVelocity -= gravity * Time.deltaTime;

    if (isGrounded())
      PlayerJump();

    moveDirection.y = verticalVelocity * Time.deltaTime;
  }

  private void PlayerJump()
  {
    if (isGrounded() && Input.GetKeyDown(KeyCode.Space))
      verticalVelocity = jumpForce;
  }
}
