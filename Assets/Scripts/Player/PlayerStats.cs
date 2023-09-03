using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class PlayerStats : CharacterStats, IDataPersistence
    {
        public HealthBar healthBar;

        PlayerManager playerManager;
        AnimatorHandler animatorHandler;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromLevel();
            healthBar.SetCurrentHealth(currentHealth);
        }

        private int SetMaxHealthFromLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void Heal(int healPoints)
        {
            if (currentHealth >= maxHealth) return;
            currentHealth += healPoints;
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void TakeDamage(int damage)
        {
            if (playerManager.isInvincible) return;
            if (isDead) return;
            currentHealth -= damage;

            healthBar.SetCurrentHealth(currentHealth);

            if (playerManager.focusFlag)
            {
                animatorHandler.PlayTargetAnimation("Damage_Focused", true);
            } else
            {
                animatorHandler.PlayTargetAnimation("Damage_Unfocused", true);
            }

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Death", true);
                isDead = true;
            }
        }


        public void LoadData(GameData data)
        {
            this.currentHealth = data.currentHealth;
        }

        public void SaveData(ref GameData data)
        {
            data.currentHealth = this.currentHealth;
        }
    }
}
