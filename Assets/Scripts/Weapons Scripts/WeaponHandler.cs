using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{

  public class WeaponHandler : MonoBehaviour
  {
    private PlayerManager playerManager;
    private Animator animator;
    [SerializeField]
    private AudioSource audioSource;
    public int bulletCount;
    public GameObject attackPoint;
    private PlayerUI playerUI;
    public WeaponData weaponData;
    public Transform shellEjectionPoint;
    [SerializeField]
    private float shellEjectionForce = 0.2f;
    [SerializeField]
    private float shellEjectionRotationForce = 10f;

    private void Start()
    {
      playerUI = FindObjectOfType<PlayerUI>();
      playerManager = FindObjectOfType<PlayerManager>();
      UpdateBulletCountUI();
    }

    void Awake()
    {
      animator = GetComponent<Animator>();
      bulletCount = weaponData.maxBulletCount;
      UpdateBulletCountUI();
    }
    private void UpdateBulletCountUI()
    {
      if (playerUI != null)
      {
        playerUI.UpdateBulletCount(bulletCount);
      }
    }

    private void OnEnable()
    {
      UpdateBulletCountUI();
    }

    public void ShootAnimation()
    {
      if (weaponData.bulletType == WeaponBulletType.NONE)
      {
        print("hit with axe");
        animator.SetTrigger(AnimationTags.SHOOT_TRIGGER);
      }
      else if (bulletCount > 0)
      {
        animator.SetTrigger(AnimationTags.SHOOT_TRIGGER);

        bulletCount--;
        UpdateBulletCountUI();
      }
    }

    public void Aim(bool canAim)
    {
      animator.SetBool(AnimationTags.AIM_PARAMETER, canAim);
    }

    #region Reload
    public void ReloadAnimation()
    {
      animator.SetTrigger(AnimationTags.RELOAD_TRIGGER);

      playerManager.isReloading = true;

      StartCoroutine(WaitForReloadAnimation());
    }

    private IEnumerator WaitForReloadAnimation()
    {
      while (!AnimatorHelper.IsAnimationPlaying(animator, AnimationTags.RELOAD_TRIGGER))
      {
        yield return null;
      }

      while (AnimatorHelper.IsAnimationPlaying(animator, AnimationTags.RELOAD_TRIGGER))
      {
        yield return null;
      }

      Reload();
    }


    void PlayReloadSound(AudioClip audioClip)
    {
      audioSource.clip = audioClip;
      audioSource.Play();
    }

    private void Reload()
    {
      bulletCount = weaponData.maxBulletCount;
      UpdateBulletCountUI();
      playerManager.isReloading = false;
    }

    #endregion

    public void Holster(bool shouldHoster)
    {
      animator.SetBool(AnimationTags.HOLSTER_PARAMETER, shouldHoster);
    }

    void TurnOnMuzzleFlash()
    {
      weaponData.muzzleFlash.SetActive(true);
    }

    void TurnOffMuzzleFlash()
    {
      weaponData.muzzleFlash.SetActive(false);
    }

    public void PlayShootSound()
    {
      audioSource.clip = weaponData.shootSoundClip;
      audioSource.Play();
    }

    void PlayPumpSound()
    {
      if (weaponData.pumpSoundClip != null)
      {
        audioSource.clip = weaponData.pumpSoundClip;
        audioSource.Play();
      }
    }

    void SpawnShell()
    {
      GameObject shell = Instantiate(weaponData.shellPrefab, shellEjectionPoint.position, shellEjectionPoint.rotation);

      Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();
      if (shellRigidbody != null)
      {
        Vector3 shellEjectionDirection = shellEjectionPoint.up +
          shellEjectionPoint.forward + shellEjectionPoint.right;

        // Apply force to move the shell away from the chamber
        shellRigidbody.AddForce(shellEjectionDirection * shellEjectionForce, ForceMode.Impulse);

        // Apply torque to give the shell some rotation
        shellRigidbody.AddRelativeTorque(Vector3.up * shellEjectionRotationForce, ForceMode.Impulse);

        StartCoroutine(DespawnShell(shell));
      }
    }

    IEnumerator DespawnShell(GameObject shell)
    {
      yield return new WaitForSeconds(10f);

      Destroy(shell);
    }

    void TurnOnAttackPoint()
    {
      attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
      if (attackPoint.activeInHierarchy)
        attackPoint.SetActive(false);
    }

  }
}