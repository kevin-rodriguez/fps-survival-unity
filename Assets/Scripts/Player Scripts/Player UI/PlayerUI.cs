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
      }
      else
      {
        bulletCountText.text = "--";
      }
    }
  }
}