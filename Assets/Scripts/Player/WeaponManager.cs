using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class WeaponManager : MonoBehaviour
    {
        WeaponHolder leftHandSlot;
        WeaponHolder rightHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        private void Awake()
        {
            WeaponHolder[] weaponHolderSlots = GetComponentsInChildren<WeaponHolder>();
            foreach (WeaponHolder weaponSlot in weaponHolderSlots)
            {
                if (weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                } 
                else
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem)
        {
            /*if (isLeftHanded)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
            }*/
            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            LoadRightWeaponDamageCollider();
        }

        

        #region Handle Damage Colliders
        public void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }
        public void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenLeftDamageCollider()
        {
            leftHandDamageCollider.EnableDamageCollider();
        }

        public void OpenRightDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseLeftDamageCollider()
        {
            leftHandDamageCollider.DisableDamageCollider();
        }

        public void CloseRightDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }

        #endregion
    }
}
