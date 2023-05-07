using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

  private WeaponManager weaponManager;
  public float fireRate = 50f;
  private float nextTimeToFire;
  public float damage = 20f;

  private Animator zoomCameraAnimator;
  private bool zoomedIn;
  private Camera mainCamera;
  private GameObject crosshair;
  private bool isAiming;
  [SerializeField]
  private GameObject arrowPrefab, spearPrefab;
  [SerializeField]
  private Transform arrowSpearStartPosition;

  void Awake()
  {
    weaponManager = GetComponent<WeaponManager>();
    zoomCameraAnimator = transform.Find(Tags.LOOK_ROOT)
                          .transform.Find(Tags.ZOOM_CAMERA)
                          .GetComponent<Animator>();
    crosshair = GameObject.FindWithTag(Tags.CROSSHAIR);
    mainCamera = Camera.main;
  }

  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    WeaponShoot();
    ZoomInOut();
  }

  void WeaponShoot()
  {
    WeaponHandler currentWeapon = weaponManager.GetCurrentSelectedWeapon();
    bool isSingleFireWeapon = currentWeapon.fireType == WeaponFireType.SINGLE;

    if (isSingleFireWeapon)
    {
      bool primaryMouseButtonClicked = Input.GetMouseButtonDown(0);

      if (primaryMouseButtonClicked)
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
      bool primaryMouseButtonHold = Input.GetMouseButton(0);
      bool isReadyToFireAgain = Time.time > nextTimeToFire;

      if (primaryMouseButtonHold && isReadyToFireAgain)
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
    bool isMouseSecodaryClickHold = Input.GetMouseButtonDown(1);
    bool isMouseSecodaryClickReleased = Input.GetMouseButtonUp(1);

    if (weaponCanAim)
    {
      if (isMouseSecodaryClickHold)
      {
        zoomCameraAnimator.Play(AnimationTags.ZOOM_IN_AIM);
        crosshair.SetActive(false);
      }
      if (isMouseSecodaryClickReleased)
      {
        zoomCameraAnimator.Play(AnimationTags.ZOOM_OUT_AIM);
        crosshair.SetActive(true);
      }
    }
    else if (weaponNeedAim)
    {
      if (isMouseSecodaryClickHold)
      {
        currentWeapon.Aim(true);
        isAiming = true;
      }
      if (isMouseSecodaryClickReleased)
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
    }
  }
}
