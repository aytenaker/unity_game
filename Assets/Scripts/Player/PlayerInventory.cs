using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponManager weaponManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;

        private void Awake()
        {
            weaponManager = GetComponentInChildren<WeaponManager>();
        }

        private void Start()
        {
            weaponManager.LoadWeaponOnSlot(rightWeapon);
        }
    }
}
