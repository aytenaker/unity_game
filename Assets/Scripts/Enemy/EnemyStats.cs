using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class EnemyStats : CharacterStats, IDataPersistence
    {
        [SerializeField] private string id;

        [ContextMenu("Generate ID")]
        private void GenerateID()
        {
            id = System.Guid.NewGuid().ToString();
        }

        public GameObject soul;
        EnemyAnimatorManager enemyAnimatorManager;

        public Transform focusTransform;
        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }
        void Start()
        {
            maxHealth = SetMaxHealthFromLevel();
            currentHealth = maxHealth;
        }

        public void LoadData(GameData data)
        {
            data.enemiesKilled.TryGetValue(id, out isDead);
            if (isDead && gameObject != null)
            {
                gameObject.SetActive(false);
            }
        }

        public void SaveData(ref GameData data)
        {
            if (data.enemiesKilled.ContainsKey(id))
            {
                data.enemiesKilled.Remove(id);
            }
            data.enemiesKilled.Add(id, isDead);
        }

        private int SetMaxHealthFromLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return;
            currentHealth -= damage;
            enemyAnimatorManager.PlayTargetAnimation("Damage", true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                enemyAnimatorManager.anim.SetBool("isInteracting", false);
                enemyAnimatorManager.PlayTargetAnimation("Death", true);
                isDead = true;
                focusTransform.gameObject.SetActive(false);

                StartCoroutine(Delete(6f));
            }
        }

        IEnumerator Delete(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            Instantiate(soul, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}
