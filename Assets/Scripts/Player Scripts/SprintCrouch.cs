using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KR
{
  public class SprintCrouch : MonoBehaviour
  {
    private WeaponManager weaponManager;
    private PlayerManager playerManager;
    private InputHandler inputHandler;
    private PlayerMovement playerMovement;
    [Header("Movement speeds")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 9f;
    public float crouchingSprintSpeed = 6f;
    public float crouchSpeed = 2f;

    private Transform lookRoot;
    private float standingHeight = 1.6f;
    private float crouchingHeight = 0.8f;
    private float crouchingHeightSpeed = 20f;

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
      playerManager = GetComponent<PlayerManager>();
      inputHandler = GetComponent<InputHandler>();
      weaponManager = GetComponent<WeaponManager>();
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
      playerMovement.speed = playerManager.isCrouching ? crouchSpeed : moveSpeed;
      playerFootsteps.stepDistance = walkStepDistance;
      playerFootsteps.volumeMinimum = walkVolumeMin;
      playerFootsteps.volumeMaximum = walkVolumeMax;
    }
    void Sprint()
    {
      bool playerHasStamina = sprintValue > 0f;
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();

      if (playerHasStamina && inputHandler.sprintInput && playerMovement.isPlayerMoving())
      {
        playerMovement.speed = playerManager.isCrouching ? crouchingSprintSpeed : sprintSpeed;
        playerFootsteps.stepDistance = sprintStepDistance;
        playerFootsteps.volumeMinimum = sprintVolume;
        playerFootsteps.volumeMaximum = sprintVolume;
        playerManager.isSprinting = true;
      }
      else
      {
        SetWalkingState();
        playerManager.isSprinting = false;
      }

      if (inputHandler.sprintInput)
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

      if (currentWeapon && currentWeapon.weaponData.bulletType == WeaponBulletType.BULLET)
      {
        currentWeapon.Holster(playerManager.isSprinting);
      }

      playerStats.DisplayStaminaStats(sprintValue);
    }

    void Crouch()
    {
      if (inputHandler.crouchInput)
      {
        if (playerManager.isCrouching)
        {
          playerMovement.speed = moveSpeed;
          playerManager.isCrouching = false;

          playerFootsteps.stepDistance = walkStepDistance;
          playerFootsteps.volumeMinimum = walkVolumeMin;
          playerFootsteps.volumeMaximum = walkVolumeMax;
        }
        else
        {
          playerMovement.speed = crouchSpeed;
          playerManager.isCrouching = true;

          playerFootsteps.stepDistance = crouchStepDistance;
          playerFootsteps.volumeMinimum = crouchVolume;
          playerFootsteps.volumeMaximum = crouchVolume;
        }
      }

      float cameraHeight = playerManager.isCrouching ? crouchingHeight : standingHeight;
      Vector3 cameraPosition = lookRoot.localPosition;
      cameraPosition.y = Mathf.SmoothStep(cameraPosition.y, cameraHeight, crouchingHeightSpeed * Time.deltaTime);
      lookRoot.localPosition = cameraPosition;
    }
  }
}