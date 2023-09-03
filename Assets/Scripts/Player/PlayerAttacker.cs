using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class PlayerAttacker : MonoBehaviour
    {
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        public string lastAttack;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
        }

        public void HandleCombo(WeaponItem weapon)
        {
            
            if (playerManager.comboFlag)
            {
                animatorHandler.anim.SetBool("canCombo", true);

                if (lastAttack == weapon.Attack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.Attack_02, true);
                    lastAttack = weapon.Attack_02;
                } else if (lastAttack == weapon.Attack_02)
                {
                    animatorHandler.PlayTargetAnimation(weapon.Attack_03, true);
                    lastAttack = weapon.Attack_03;
                }
                else if (lastAttack == weapon.Attack_03)
                {
                    animatorHandler.PlayTargetAnimation(weapon.Attack_04, true);
                    lastAttack = weapon.Attack_04;
                }
                else if (lastAttack == weapon.Attack_04)
                {
                    animatorHandler.PlayTargetAnimation(weapon.Attack_05, true);
                    lastAttack = weapon.Attack_05;
                }

            }
            
        }

        public void HandleSpecialCombo(WeaponItem weapon)
        {
            if (playerManager.comboFlag)
            {
                animatorHandler.anim.SetBool("canCombo", true);

                if (lastAttack == weapon.SpecialAttack_01)
                {
                    animatorHandler.PlayTargetAnimation(weapon.SpecialAttack_02, true);
                    lastAttack = weapon.SpecialAttack_02;
                }
                else if (lastAttack == weapon.SpecialAttack_02)
                {
                    animatorHandler.PlayTargetAnimation(weapon.SpecialAttack_03, true);
                    lastAttack = weapon.SpecialAttack_03;
                }

            }

        }

        public void HandleAttack(WeaponItem weapon)
        {
            if (cameraHandler.gameOverMenu.isActive || cameraHandler.finishMenu.isActive) return;
            if (!playerManager.isInteracting)
            {
                animatorHandler.PlayTargetAnimation(weapon.Attack_01, true);
                lastAttack = weapon.Attack_01;
            }
        }

        public void HandleSpecialAttack(WeaponItem weapon)
        {
            if (cameraHandler.gameOverMenu.isActive || cameraHandler.finishMenu.isActive) return;
            if (!playerManager.isInteracting)
            {
                animatorHandler.PlayTargetAnimation(weapon.SpecialAttack_01, true);
                lastAttack = weapon.SpecialAttack_01;
            }
        }
    }
}
