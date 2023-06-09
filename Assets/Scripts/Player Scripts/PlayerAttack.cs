using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class PlayerAttack : MonoBehaviour
  {
    private InputHandler inputHandler;
    private PlayerManager playerManager;
    private WeaponManager weaponManager;
    private float nextTimeToFire;
    public float baseDamage = 1;

    private Animator zoomCameraAnimator;
    private bool zoomedIn;
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
    [SerializeField]
    private RecoilSystem recoilSystem;

    void Start()
    {
      playerManager = GetComponent<PlayerManager>();
    }

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
        playerManager.isAiming = inputHandler.aimInput;
        WeaponShoot();
        ZoomInOut();
        HandleReload();
      }
    }

    void WeaponShoot()
    {
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
      bool isSingleFireWeapon = currentWeapon.weaponData.fireType == WeaponFireType.SINGLE;
      bool isReadyToFireAgain = Time.time > nextTimeToFire;

      if (currentWeapon.bulletCount > 0 && !playerManager.isReloading && !playerManager.isSprinting)
      {

        if (isSingleFireWeapon)
        {
          if (inputHandler.attackInput && isReadyToFireAgain)
          {
            nextTimeToFire = Time.time + 1f / currentWeapon.weaponData.fireRate;

            // Handle Axe Attack
            if (currentWeapon.CompareTag(Tags.AXE_TAG))
              currentWeapon.ShootAnimation();

            // Handle Bullet Attack
            if (currentWeapon.weaponData.bulletType == WeaponBulletType.BULLET)
            {
              currentWeapon.ShootAnimation();
              BulletFired();
            }
            else // Arrow/Spear
            {
              if (playerManager.isAiming)
              {
                currentWeapon.ShootAnimation();

                if (currentWeapon.weaponData.bulletType == WeaponBulletType.ARROW)
                {
                  ThrowArrowOrSpear(arrowPrefab);
                }
                else if (currentWeapon.weaponData.bulletType == WeaponBulletType.SPEAR)
                {
                  ThrowArrowOrSpear(spearPrefab);
                }
              }
            }
          }
        }
        else
        {
          if (inputHandler.attackInput && isReadyToFireAgain)
          {
            nextTimeToFire = Time.time + 1f / currentWeapon.weaponData.fireRate;

            currentWeapon.ShootAnimation();

            BulletFired();
          }
        }
      }
    }

    void ZoomInOut()
    {
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
      bool weaponCanAim = currentWeapon.weaponData.weaponAim == WeaponAim.AIM;
      bool weaponNeedAim = currentWeapon.weaponData.weaponAim == WeaponAim.SELF_AIM;

      if (weaponCanAim)
      {
        if (inputHandler.aimInput)
        {
          zoomCameraAnimator.Play(AnimationTags.ZOOM_IN_AIM);
        }
        else
        {
          zoomCameraAnimator.Play(AnimationTags.ZOOM_OUT_AIM);
        }
      }
      else if (weaponNeedAim)
      {
        currentWeapon.Aim(inputHandler.aimInput);
      }
    }

    void ThrowArrowOrSpear(GameObject projectileType)
    {
      GameObject projectile = Instantiate(projectileType);
      projectile.transform.position = arrowSpearStartPosition.position;
      projectile.GetComponent<ArrowSpear>().Launch(mainCamera);
      projectile.GetComponent<ArrowSpear>().onHit += Hit;
    }

    void BulletFired()
    {
      RaycastHit hit;

      recoilSystem.ApplyRecoil();

      if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
      {
        WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();

        Hit(hit.transform);

        decalPlacer.GetComponent<DecalController>().SpawnDecal(hit, hit.transform.tag, currentWeapon.weaponData.weaponDamage / 3, hit.transform.CompareTag(Tags.ENEMY_TAG));
      }
    }

    private void HandleReload()
    {
      if (inputHandler.reloadInput)
      {
        WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
        currentWeapon.ReloadAnimation();
      }
    }

    void Hit(Transform targetTransform)
    {
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();

      if (targetTransform.tag == Tags.ENEMY_TAG)
      {
        targetTransform.GetComponent<EnemyAnimator>().Hit();
        targetTransform.GetComponent<HealthScript>().ApplyDamage(baseDamage * currentWeapon.weaponData.weaponDamage);
      }
    }

    void OnDrawGizmos()
    {
      RaycastHit hit;
      Gizmos.color = Color.blue;

      if (mainCamera != null)
      {
        // Draw line when user fire and add a sphere on the collider (for debugging purposes)
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * 500);

        if (Physics.Raycast(ray, out hit))
        {
          Gizmos.DrawSphere(hit.point, 0.1f);
        }
      }

    }
  }
}