using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class WeaponManager : MonoBehaviour
  {

    [SerializeField]
    private WeaponHandler[] weapons;
    private PlayerManager playerManager;
    private int currentWeaponIndex;

    void Start()
    {
      playerManager = GetComponent<PlayerManager>();

      playerManager.isUnarmed = true;
    }

    void Update()
    {
      if (Input.GetKeyDown(KeyCode.Alpha1))
        TurnOnSelectedWeapon(0);

      if (Input.GetKeyDown(KeyCode.Alpha2))
        TurnOnSelectedWeapon(1);


      if (Input.GetKeyDown(KeyCode.Alpha3))
        TurnOnSelectedWeapon(2);


      if (Input.GetKeyDown(KeyCode.Alpha4))
        TurnOnSelectedWeapon(3);


      if (Input.GetKeyDown(KeyCode.Alpha5))
        TurnOnSelectedWeapon(4);


      if (Input.GetKeyDown(KeyCode.Alpha6))
        TurnOnSelectedWeapon(5);

      if (Input.GetKeyDown(KeyCode.Alpha7))
        TurnOnSelectedWeapon(6);
    }

    void TurnOnSelectedWeapon(int weaponIndex)
    {
      if (playerManager.isUnarmed)
      {
        playerManager.isUnarmed = false;
        currentWeaponIndex = 0;
      }

      if (currentWeaponIndex != weaponIndex)
      {
        weapons[currentWeaponIndex].gameObject.SetActive(false);
        weapons[weaponIndex].gameObject.SetActive(true);

        currentWeaponIndex = weaponIndex;
      }
    }

    public WeaponHandler GetCurrentSelectedWeapon()
    {
      if (playerManager.isUnarmed)
        return null;

      return weapons[currentWeaponIndex];
    }
  }
}