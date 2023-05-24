using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class MouseLook : MonoBehaviour
  {
    InputHandler inputHandler;

    [SerializeField]
    private Transform playerRoot, lookRoot;
    [SerializeField]
    private bool canUnlock = true;
    [SerializeField]
    private float sensitivity = 0.5f;
    [SerializeField]
    private int smoothSteps = 10;
    [SerializeField]
    private float smoothWeight = 0.4f;
    [SerializeField]
    private float rollAngle = 0.5f;
    private float rollSpeed = 3f;

    [SerializeField]
    private Vector2 defaultLookLimits = new Vector2(-70f, 80f);
    private Vector2 lookAngles;
    private Vector2 currentMouseLook;
    private Vector2 smoothMove;
    private float currentRollAngle;
    private int lastLookFrame;
    private Quaternion targetLookRootRotation;

    void Start()
    {
      inputHandler = GetComponentInParent<InputHandler>();
    }

    private void Update()
    {
      LockAndUnlockCursor();
    }

    private void LockAndUnlockCursor()
    {
      if (inputHandler.pauseInput)
      {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
          Cursor.lockState = CursorLockMode.None;
        }
        else
        {
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
        }
      }
    }

    public void LookAround()
    {

      if (Cursor.lockState == CursorLockMode.Locked)
      {

        currentMouseLook = new Vector2(inputHandler.mouseY, inputHandler.mouseX);

        lookAngles.x += currentMouseLook.x * sensitivity * -1;
        lookAngles.y += currentMouseLook.y * sensitivity;

        // Clamp look between two angles
        lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);

        // Make camera rotate a little
        currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw(MouseAxis.MOUSE_X)
                                        * rollAngle, Time.deltaTime * rollSpeed);

        if (currentMouseLook != Vector2.zero)
        {
          lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
          playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);

          targetLookRootRotation = lookRoot.localRotation;
        }
        else
        {
          lookRoot.localRotation = Quaternion.Lerp(lookRoot.localRotation, targetLookRootRotation, smoothWeight);
        }
      }
    }

  }
}