using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
  private AudioSource audioSource;
  [SerializeField]
  private AudioClip screamClip, deathClip;
  [SerializeField]
  private AudioClip[] attackClips;

  void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public void PlayScreamSound()
  {
    audioSource.clip = screamClip;
    audioSource.Play();
  }

  public void PlayAttackSound()
  {
    if (!audioSource.isPlaying)
    {
      int randomIndex = Random.Range(0, attackClips.Length);

      audioSource.clip = attackClips[randomIndex];


      audioSource.Play();
    }
  }

  public void PlayDeathSound()
  {
    audioSource.clip = deathClip;
    audioSource.Play();
  }
}
