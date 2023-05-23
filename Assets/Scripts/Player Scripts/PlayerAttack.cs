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
    public float damage = 20f;

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
      }
    }

    void WeaponShoot()
    {
      WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
      bool isSingleFireWeapon = currentWeapon.fireType == WeaponFireType.SINGLE;
      bool isReadyToFireAgain = Time.time > nextTimeToFire;

      if (isSingleFireWeapon)
      {
        if (inputHandler.attackInput && isReadyToFireAgain)
        {
          nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;

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
            if (playerManager.isAiming)
            {
              currentWeapon.ShootAnimation();

              if (currentWeapon.bulletType == WeaponBulletType.ARROW)
              {
                ThrowArrowOrSpear(arrowPrefab);
              }
              else if (currentWeapon.bulletType == WeaponBulletType.SPEAR)
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
          nextTimeToFire = Time.time + 1f / currentWeapon.fireRate;

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

      if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit))
      {
        Hit(hit.transform);

        if (hit.transform.tag != Tags.ENEMY_TAG)
        {
          // Only displaying bullet holes for static objects for now
          decalPlacer.GetComponent<DecalController>().SpawnDecal(hit);
        }
      }
    }

    void Hit(Transform targetTransform)
    {
      if (targetTransform.tag == Tags.ENEMY_TAG)
      {
        targetTransform.GetComponent<EnemyAnimator>().Hit();
        targetTransform.GetComponent<HealthScript>().ApplyDamage(damage);
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