using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
  private AudioSource footstepSound;
  [SerializeField]
  private AudioClip[] footstepClips;
  private CharacterController characterController;
  [HideInInspector]
  public float volumeMinimum, volumeMaximum;
  private float accumulatedDistance;
  [HideInInspector]
  public float stepDistance;

  void Awake()
  {
    footstepSound = GetComponent<AudioSource>();
    characterController = GetComponentInParent<CharacterController>();
  }

  void Update()
  {
    CheckToPlayFootstepSound();
  }

  void CheckToPlayFootstepSound()
  {
    if (characterController.isGrounded)
    {
      bool isPlayerMoving = characterController.velocity.sqrMagnitude > 0;

      if (isPlayerMoving)
      {

        accumulatedDistance += Time.deltaTime;

        if (accumulatedDistance > stepDistance)
        {
          footstepSound.volume = Random.Range(volumeMinimum, volumeMaximum);
          footstepSound.clip = footstepClips[Random.Range(0, footstepClips.Length)];

          footstepSound.Play();
          accumulatedDistance = 0;
        }
      }
      else
      {
        accumulatedDistance = 0;
      }
    }
  }

}
