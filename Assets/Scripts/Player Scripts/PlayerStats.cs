using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
  public Image healthBar, staminaBar;
  private HealthScript playerHealth;

  public void DisplayHealthStats(float healthValue)
  {
    healthValue /= 100f;
    healthBar.fillAmount = healthValue;
  }
  public void DisplayStaminaStats(float staminaValue)
  {
    staminaValue /= 100f;
    staminaBar.fillAmount = staminaValue;
  }
}
