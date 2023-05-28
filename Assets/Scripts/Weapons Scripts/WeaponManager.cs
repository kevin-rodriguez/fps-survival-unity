using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class WeaponManager : MonoBehaviour
  {

    [SerializeField] 
    private WeaponHandler primaryWeapon;

    [SerializeField] 
    private WeaponHandler secondaryWeapon;
    
    private PlayerManager playerManager;
    
    public WeaponHandler currentWeapon;

    void Start()
    {
      playerManager = GetComponent<PlayerManager>();

      playerManager.isUnarmed = true;
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        HandleWeaponChange(primaryWeapon);

      if (Input.GetKeyDown(KeyCode.Alpha2))
        HandleWeaponChange(secondaryWeapon);
    }

    public void AddWeapon(WeaponHandler weaponToAdd)
    {
      if (playerManager.isUnarmed)
      {
        playerManager.isUnarmed = false;
      }
      
      if (currentWeapon != null)
      {
        Destroy(currentWeapon.gameObject);
      }

      currentWeapon = weaponToAdd;
    }
    
    void HandleWeaponChange(WeaponHandler weapon)
    {
      
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
      if (playerManager.isUnarmed)
        return null;

      return currentWeapon;
      //return weapons[currentWeapon];
    }
  }
}