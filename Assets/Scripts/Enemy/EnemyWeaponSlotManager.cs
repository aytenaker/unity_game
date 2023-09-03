using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;

        WeaponHolder rightHandSlot;
        WeaponHolder leftHandSlot;

        DamageCollider rightHandDamageCollider;
        DamageCollider leftHandDamageCollider;

        private void Awake()
        {
            WeaponHolder[] weaponHolders = GetComponentsInChildren<WeaponHolder>();
            foreach (WeaponHolder weaponSlot in weaponHolders)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                } else if (weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        private void Start()
        {
            LoadWeapons();
        }

        public void LoadWeaponInSlot(WeaponItem weapon, bool isLeftHanded)
        {
            if (isLeftHanded)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(false);
            }
        }

        public void LoadWeapons()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponInSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponInSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponsDamageCollider(bool isLeftHanded)
        {
            if (isLeftHanded)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        public void OpenDamageColliders()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageColliders()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
    }
}
