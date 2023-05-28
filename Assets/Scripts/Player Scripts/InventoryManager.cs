using UnityEngine;

namespace KR
{
    public class InventoryManager : MonoBehaviour
    {
        private WeaponManager weaponManager;

        private void Start()
        {
            weaponManager = GetComponent<WeaponManager>();
            
        }
    }
}
