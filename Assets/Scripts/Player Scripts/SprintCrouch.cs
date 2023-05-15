using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintCrouch : MonoBehaviour
{
  private PlayerMovement playerMovement;
  public float moveSpeed = 6f;
  public float sprintSpeed = 9f;
  public float crouchingSprintSpeed = 7f;
  public float crouchSpeed = 2f;

  private Transform lookRoot;
  private float standingHeight = 1.6f;
  private float crouchingHeight = 1f;

  private bool isCrouching;
  private PlayerFootsteps playerFootsteps;
  private float sprintVolume = 1f;
  private float crouchVolume = 0.1f;
  private float crouchingSprintVolume = 0.3f;
  private float walkVolumeMin = 0.2f, walkVolumeMax = 0.6f;
  private float walkStepDistance = 0.5f;
  private float sprintStepDistance = 0.3f;
  private float crouchStepDistance = 0.6f;

  private PlayerStats playerStats;
  private float sprintValue = 100f;
  public float sprintTreshold = 10f;


  void Awake()
  {
    playerMovement = GetComponent<PlayerMovement>();
    lookRoot = transform.GetChild(0);
    playerFootsteps = GetComponentInChildren<PlayerFootsteps>();
    playerStats = GetComponent<PlayerStats>();
  }

  void Start()
  {
    playerFootsteps.volumeMinimum = walkVolumeMin;
    playerFootsteps.volumeMaximum = walkVolumeMax;
    playerFootsteps.stepDistance = walkStepDistance;
  }

  void Update()
  {
    Sprint();
    Crouch();
  }

  void SetWalkingState()
  {
    playerMovement.speed = moveSpeed;
    playerFootsteps.stepDistance = walkStepDistance;
    playerFootsteps.volumeMinimum = walkVolumeMin;
    playerFootsteps.volumeMaximum = walkVolumeMax;
  }
  void Sprint()
  {
    bool playerHasStamina = sprintValue > 0f;

    if (playerHasStamina && Input.GetKeyDown(KeyCode.LeftShift))
    {
      playerMovement.speed = isCrouching ? crouchingSprintSpeed : sprintSpeed;
      playerFootsteps.stepDistance = sprintStepDistance;
      playerFootsteps.volumeMinimum = sprintVolume;
      playerFootsteps.volumeMaximum = sprintVolume;
    }

    if (Input.GetKeyUp(KeyCode.LeftShift))
    {
      SetWalkingState();
    }

    if (Input.GetKey(KeyCode.LeftShift))
    {
      sprintValue -= sprintTreshold * Time.deltaTime;

      if (sprintValue <= 0f)
      {
        sprintValue = 0f;
        SetWalkingState();
      }
    }
    else if (sprintValue != 100f)
    {
      sprintValue += (sprintTreshold / 2f) * Time.deltaTime;

      if (sprintValue > 100f)
      {
        sprintValue = 100f;
      }
    }
    playerStats.DisplayStaminaStats(sprintValue);
  }

  void Crouch()
  {
    if (Input.GetKeyDown(KeyCode.C))
    {
      if (isCrouching)
      {
        lookRoot.localPosition = new Vector3(0f, standingHeight, 0f);
        playerMovement.speed = moveSpeed;
        isCrouching = false;

        playerFootsteps.stepDistance = walkStepDistance;
        playerFootsteps.volumeMinimum = walkVolumeMin;
        playerFootsteps.volumeMaximum = walkVolumeMax;
      }
      else
      {
        lookRoot.localPosition = new Vector3(0f, crouchingHeight, 0f);
        playerMovement.speed = crouchSpeed;
        isCrouching = true;

        playerFootsteps.stepDistance = crouchStepDistance;
        playerFootsteps.volumeMinimum = crouchVolume;
        playerFootsteps.volumeMaximum = crouchVolume;
      }
    }
  }
}
