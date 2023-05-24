using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public enum WeaponAim
  {
    NONE,
    SELF_AIM,
    AIM
  }

  public enum WeaponFireType
  {
    SINGLE,
    MULTIPLE
  }

  public enum WeaponBulletType
  {
    NONE,
    ARROW,
    SPEAR,
    BULLET
  }

  public class WeaponHandler : MonoBehaviour
  {
    private PlayerManager playerManager;
    private Animator animator;
    public WeaponAim weaponAim;
    [SerializeField]
    private GameObject muzzleFlash;
    public WeaponFireType fireType;
    public int fireRate = 1;
    public WeaponBulletType bulletType;

    [Header("Bullets")]
    [SerializeField]
    private int maxBulletCount = 10;
    public int bulletCount;

    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip shootSoundClip;
    [SerializeField]
    private AudioClip reloadSoundClip;
    public GameObject attackPoint;
    private PlayerUI playerUI;

    private void Start()
    {
      playerUI = FindObjectOfType<PlayerUI>();
      playerManager = FindObjectOfType<PlayerManager>();
      UpdateBulletCountUI();
    }

    void Awake()
    {
      animator = GetComponent<Animator>();
      bulletCount = maxBulletCount;
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
      if (bulletCount > 0)
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

    public void ReloadAnimation()
    {
      animator.SetTrigger(AnimationTags.RELOAD_TRIGGER);
      PlayReloadSound();
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

    private void Reload()
    {
      bulletCount = maxBulletCount;
      UpdateBulletCountUI();
      playerManager.isReloading = false;
    }

    public void Holster(bool shouldHoster)
    {
      animator.SetBool(AnimationTags.HOLSTER_PARAMETER, shouldHoster);
    }

    void TurnOnMuzzleFlash()
    {
      muzzleFlash.SetActive(true);
    }

    void TurnOffMuzzleFlash()
    {
      muzzleFlash.SetActive(false);
    }

    public void PlayShootSound()
    {
      audioSource.clip = shootSoundClip;
      audioSource.Play();
    }

    void PlayReloadSound()
    {
      //audioSource.clip = reloadSoundClip;
      //audioSource.Play();
      //reloadSound.Play();
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