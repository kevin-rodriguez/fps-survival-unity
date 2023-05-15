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

    void Start()
    {
      inputHandler = GetComponent<InputHandler>();
      playerMovement = GetComponent<PlayerMovement>();
      mouseLook = GetComponentInChildren<MouseLook>();
    }

    void Update()
    {
      float delta = Time.deltaTime;

      inputHandler.TickInput(delta);

      mouseLook.LookAround();
    }

    void FixedUpdate()
    {
      playerMovement.MovePlayer();
    }
  }

}