using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class AnimationEventHandler : MonoBehaviour
    {
        PlayerLocomotion playerLocomotion;
        Renderer renderer;
        WeaponHolder leftHandSlot;
        WeaponHolder rightHandSlot;
        ParticleSystem in_vfx;
        ParticleSystem out_vfx;
        ParticleSystem weapon_trail;

        private void Awake()
        {
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            renderer = GetComponentInChildren<Renderer>();

            in_vfx = GameObject.Find("in").GetComponentInChildren<ParticleSystem>();
            out_vfx = GameObject.Find("out").GetComponentInChildren<ParticleSystem>();


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

        public void ShowModel()
        {
            renderer.enabled = true;
        }

        public void HideModel()
        {
            renderer.enabled = false;
        }

        public void HideWeapon()
        {
            rightHandSlot.HideWeapon();
        }

        public void ShowWeapon()
        {
            rightHandSlot.ShowWeapon();
        }

        public void FadeIn()
        {
            in_vfx.Play();
        }

        public void FadeOut()
        {
            out_vfx.Play();
        }

        public void ActivateWeaponTrail()
        {
            weapon_trail = GetComponentInChildren<ParticleSystem>();
            weapon_trail.Play();
        }

        public void DeactivateWeaponTrail()
        {
            if (weapon_trail != null) weapon_trail.Stop();
        }

        public void PushTowardsTarget()
        {
            playerLocomotion.rigidbody.AddForce(transform.forward * 30, ForceMode.VelocityChange);
        }

    }
}
