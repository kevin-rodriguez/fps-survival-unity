using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class WeaponManager : MonoBehaviour
  {
    [SerializeField] private int maxWeaponSlots = 2;
    public List<WeaponHandler> weaponSlots;
    
    private PlayerManager playerManager;
    
    public WeaponHandler currentWeapon;

    void Start()
    {
      playerManager = GetComponent<PlayerManager>();

      playerManager.isUnarmed = true;
    }

    void Update()
    {
      //if (Input.GetKeyDown(KeyCode.Alpha1))
        //HandleWeaponChange(primaryWeapon);

      //if (Input.GetKeyDown(KeyCode.Alpha2))
        //HandleWeaponChange(secondaryWeapon);
    }

    public void AddWeapon(WeaponHandler weapon)
    {
      if (playerManager.isUnarmed)
      {
        playerManager.isUnarmed = false;
      }

      if (weaponSlots.Count < maxWeaponSlots)
      {
        weaponSlots.Add((weapon));
      }
      else
      {
        Destroy(weapon.gameObject);
      }
      
    }

    /*public bool EquipWeapon(WeaponHandler weapon)
    {
     
      
      
    }*/

    public void UnequipWeapon(WeaponHandler weapon)
    {
      if (weaponSlots.Contains(weapon))
      {
        weaponSlots.Remove(weapon);
        Debug.Log("Unequipped " + weapon.weaponData.name);
      }
    }

    void HandleWeaponChange(WeaponHandler weapon)
    {
      
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
      return playerManager.isUnarmed ? null : currentWeapon;
    }

    
  }
}