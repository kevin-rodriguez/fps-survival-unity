using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KR
{
  public class Crosshair : MonoBehaviour
  {
    [SerializeField]
    private GameObject aimCrosshair;
    [SerializeField]
    private GameObject interactCrosshair;
    [SerializeField]
    private GameObject player;
    private PlayerManager playerManager;

    void Start()
    {
      playerManager = player.GetComponent<PlayerManager>();
    }

    void Update()
    {
      if (playerManager.isUnarmed && !playerManager.isInteracting ||
          playerManager.isAiming ||
          playerManager.isReloading ||
          playerManager.isSprinting)
      {
        HideCrosshair();
        return;
      }

      if (playerManager.isInteracting)
      {
        SetInteractCrosshair();
      }
      else
      {
        SetAimCrosshair();
      }
    }

    private void SetAimCrosshair()
    {
      aimCrosshair.SetActive(true);
      interactCrosshair.SetActive(false);
    }

    private void SetInteractCrosshair()
    {
      interactCrosshair.SetActive(true);
      aimCrosshair.SetActive(false);
    }

    private void HideCrosshair()
    {
      interactCrosshair.SetActive(false);
      aimCrosshair.SetActive(false);
    }
  }
}