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
    private Animator animator;
    public WeaponAim weaponAim;
    [SerializeField]
    private GameObject muzzleFlash;
    public WeaponFireType fireType;
    public int fireRate = 1;
    public WeaponBulletType bulletType;

    [Header("Bullets")]
    [SerializeField]
    public int maxBulletCount = 10;
    private int bulletCount;

    [SerializeField]
    private AudioSource shootSound, reloadSound;
    public GameObject attackPoint;
    private PlayerUI playerUI;

    private void Start()
    {
      playerUI = FindObjectOfType<PlayerUI>();
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
      animator.SetTrigger(AnimationTags.SHOOT_TRIGGER);

      if (bulletCount > 0)
      {
        bulletCount--;
      }
      else
      {
        //Reload
        bulletCount = maxBulletCount;
      }
      UpdateBulletCountUI();
    }

    public void Aim(bool canAim)
    {
      animator.SetBool(AnimationTags.AIM_PARAMETER, canAim);
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
      shootSound.Play();
    }

    void PlayReloadSound()
    {
      reloadSound.Play();
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