using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KR
{
  public class PlayerManager : MonoBehaviour
  {
    InputHandler inputHandler;
    PlayerMovement playerMovement;
    MouseLook mouseLook;

    [Header("Player Flags")]
    public bool isInteracting;
    public bool isSprinting;
    public bool isCrouching;
    public bool isInAir;
    public bool isGrounded;
    public bool isUnarmed;
    public bool isAiming;

    void Start()
    {
      inputHandler = GetComponent<InputHandler>();
      playerMovement = GetComponent<PlayerMovement>();
      mouseLook = GetComponentInChildren<MouseLook>();
      isUnarmed = true;
    }

    void Update()
    {
      float delta = Time.deltaTime;

      inputHandler.TickInput(delta);

      mouseLook.LookAround();

      playerMovement.HandleJump();
    }

    void FixedUpdate()
    {
      playerMovement.MovePlayer();
    }

    private void LateUpdate()
    {
      float delta = Time.deltaTime;

      inputHandler.crouchInput = false;
      inputHandler.jumpInput = false;
      inputHandler.attackInput = false;
      inputHandler.pauseInput = false;
      inputHandler.interactInput = false;
    }

  }

}