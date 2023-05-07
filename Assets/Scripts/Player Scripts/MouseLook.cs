using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

  [SerializeField]
  private Transform playerRoot, lookRoot;

  [SerializeField]
  private bool canUnlock = true;

  [SerializeField]
  private float sensitivity = 5f;

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

  void Start()
  {
    Cursor.lockState = CursorLockMode.Locked;
  }

  void Update()
  {
    LookAround();
  }

  void LookAround()
  {
    float mouseInputAxisY = Input.GetAxis(MouseAxis.MOUSE_Y);
    float mouseInputAxisX = Input.GetAxis(MouseAxis.MOUSE_X);

    currentMouseLook = new Vector2(mouseInputAxisY, mouseInputAxisX);

    lookAngles.x += currentMouseLook.x * sensitivity * -1;
    lookAngles.y += currentMouseLook.y * sensitivity;

    // Clamp look between two angles
    lookAngles.x = Mathf.Clamp(lookAngles.x, defaultLookLimits.x, defaultLookLimits.y);

    // Make camera rotate a little
    currentRollAngle = Mathf.Lerp(currentRollAngle, Input.GetAxisRaw(MouseAxis.MOUSE_X)
                                    * rollAngle, Time.deltaTime * rollSpeed);

    lookRoot.localRotation = Quaternion.Euler(lookAngles.x, 0f, currentRollAngle);
    playerRoot.localRotation = Quaternion.Euler(0f, lookAngles.y, 0f);
  }


}
