using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerAttack : MonoBehaviour
  {

    private InputHandler inputHandler;
    private WeaponManager weaponManager;
    public float fireRate = 50f;
    private float nextTimeToFire;
    public float damage = 20f;

    private Animator zoomCameraAnimator;
    private bool zoomedIn;
    private bool isAiming;
    private Camera mainCamera;
    [SerializeField]
    private GameObject crosshair;
    [SerializeField]
    private GameObject arrowPrefab, spearPrefab;
    [SerializeField]
    private Transform arrowSpearStartPosition;
    [SerializeField]
    private GameObject decalPlacer;
    private PlayerUI playerUI;

    void Awake()
    {
      weaponManager = GetComponent<WeaponManager>();
      zoomCameraAnimator = transform.Find(Tags.LOOK_ROOT)
                            .transform.Find(Tags.ZOOM_CAMERA)
                            .GetComponent<Animator>();
      inputHandler = GetComponent<InputHandler>();
      mainCamera = Camera.main;
    }

    void Update()
    {
      if (weaponManager.GetCurrentSelectedWeapon() != null)
      {
        WeaponShoot();
        ZoomInOut();
      }
    }

    void WeaponShoot()
    {
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
      bool isSingleFireWeapon = currentWeapon.fireType == WeaponFireType.SINGLE;

      if (isSingleFireWeapon)
      {
        if (inputHandler.attackInput)
        {
          // Handle Axe Attack
          if (currentWeapon.CompareTag(Tags.AXE_TAG))
            currentWeapon.ShootAnimation();

          // Handle Bullet Attack
          if (currentWeapon.bulletType == WeaponBulletType.BULLET)
          {
            currentWeapon.ShootAnimation();
            BulletFired();
          }
          else // Arrow/Spear
          {
            if (isAiming)
            {
              currentWeapon.ShootAnimation();

              if (currentWeapon.bulletType == WeaponBulletType.ARROW)
              {
                ThrowErrorOrSpear(true);
              }
              else if (currentWeapon.bulletType == WeaponBulletType.SPEAR)
              {
                ThrowErrorOrSpear(false);
              }
            }
          }
        }
      }
      else
      {
        bool isReadyToFireAgain = Time.time > nextTimeToFire;

        if (inputHandler.attackInput && isReadyToFireAgain)
        {
          nextTimeToFire = Time.time + 1f / fireRate;

          currentWeapon.ShootAnimation();

          BulletFired();
        }
      }
    }

    void ZoomInOut()
    {
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
      bool weaponCanAim = currentWeapon.weaponAim == WeaponAim.AIM;
      bool weaponNeedAim = currentWeapon.weaponAim == WeaponAim.SELF_AIM;

      if (weaponCanAim)
      {
        if (inputHandler.aimInput)
        {
          zoomCameraAnimator.Play(AnimationTags.ZOOM_IN_AIM);
          crosshair.SetActive(false);
        }
        else
        {
          zoomCameraAnimator.Play(AnimationTags.ZOOM_OUT_AIM);
        }
      }
      else if (weaponNeedAim)
      {
        if (inputHandler.aimInput)
        {
          currentWeapon.Aim(true);
          isAiming = true;
        }
        else
        {
          currentWeapon.Aim(false);
          isAiming = false;
        }
      }
    }

    void ThrowErrorOrSpear(bool throwArrow)
    {
      GameObject projectileType;

      if (throwArrow)
        projectileType = arrowPrefab;
      else
        projectileType = spearPrefab;

      GameObject projectile = Instantiate(projectileType);
      projectile.transform.position = arrowSpearStartPosition.position;
      projectile.GetComponent<ArrowSpear>().Launch(mainCamera);
    }

    void BulletFired()
    {
      RaycastHit hit;

      if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
      {
        if (hit.transform.tag == Tags.ENEMY_TAG)
        {
          hit.transform.GetComponent<EnemyAnimator>().Hit();
          hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);


        }
        print("Hit " + hit.transform.name);
        decalPlacer.GetComponent<DecalPlacer>().SpawnDecal(hit);
      }
    }

    void OnDrawGizmos()
    {
      RaycastHit hit;
      Gizmos.color = Color.blue;

      if (mainCamera != null)
      {
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
        {
          Gizmos.DrawSphere(hit.point, 0.1f);
        }
      }

    }
  }
}