using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{

  [SerializeField]
  private WeaponHandler[] weapons;
  private int currentWeaponIndex;

  void Start()
  {
    currentWeaponIndex = 0;
    weapons[currentWeaponIndex].gameObject.SetActive(true);
  }

  // Update is called once per frame
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
    if (currentWeaponIndex != weaponIndex)
    {
      weapons[currentWeaponIndex].gameObject.SetActive(false);
      weapons[weaponIndex].gameObject.SetActive(true);

      currentWeaponIndex = weaponIndex;
    }
  }

  public WeaponHandler GetCurrentSelectedWeapon()
  {
    return weapons[currentWeaponIndex];
  }
}
