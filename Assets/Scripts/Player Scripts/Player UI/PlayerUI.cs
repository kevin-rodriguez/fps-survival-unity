using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace KR
{
  public class PlayerUI : MonoBehaviour
  {

    [SerializeField]
    private TextMeshProUGUI promptText;
    [SerializeField]
    private TextMeshProUGUI bulletCountText;
    private WeaponManager weaponManager;
    [SerializeField] 
    private WeaponInventorySlot weaponInventorySlot;

    private void Start()
    {
      weaponManager = GetComponent<WeaponManager>();
    }

    public void UpdateText(string promptMessage)
    {
      promptText.text = promptMessage;
    }

    public void ClearText()
    {
      promptText.text = string.Empty;
    }

    public void UpdateBulletCount(int bulletCount)
    {
      if (bulletCount > 0)
      {
        bulletCountText.text = bulletCount.ToString();
        bulletCountText.color = Color.white;
      }
      else
      {
        bulletCountText.text = "--";
        bulletCountText.color = Color.red;
      }
    }

    private void Update()
    {
      UpdateInventorySlot();
    }

    public void UpdateInventorySlot()
    {
      foreach (WeaponHandler weapon in weaponManager.weaponSlots)
      {
        weaponInventorySlot.AddItem(weapon);
      }
    }
  }
}