using UnityEngine;
using UnityEngine.UI;

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

  [CreateAssetMenu(fileName = "New WeaponData", menuName = "Weapons/Weapon Data")]
  public class WeaponData : ScriptableObject
  {
    [Header("General")]
    public string weaponName;
    public float weaponDamage;
    [Header("Bullet Settings")]
    public int maxBulletCount;
    public GameObject shellPrefab;

    [Header("Sounds")]
    public AudioClip shootSoundClip;
    public AudioClip pumpSoundClip;

    [Header("Aim Settings")]
    public WeaponAim weaponAim;
    public GameObject muzzleFlash;

    [Header("Fire Settings")]
    public WeaponFireType fireType;
    public int fireRate;

    [Header("Bullet Settings")]
    public WeaponBulletType bulletType;

    [Header("Icon")] 
    public Sprite weaponIcon;
  }
}