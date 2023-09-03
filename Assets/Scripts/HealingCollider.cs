using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace game
{
    public class HealingCollider : MonoBehaviour
    {
        Collider collider;

        public int healPoints;

        private void Awake()
        {
            collider = GetComponent<BoxCollider>();
            collider.isTrigger = true;
            
        }

        private void Start()
        {
            StartCoroutine(Delete(10f));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                PlayerStats playerStats = other.GetComponent<PlayerStats>();

                if (playerStats != null) {
                    if (playerStats.currentHealth >= 100) return;
                    playerStats.Heal(healPoints);
                    if (playerStats.currentHealth > 100)
                    {
                        playerStats.currentHealth = 100;
                        playerStats.healthBar.SetCurrentHealth(100);
                    }
                    Destroy(gameObject);
                }
            }
        }

        IEnumerator Delete(float seconds)
        {
            yield return new WaitForSeconds(seconds);

            Destroy(gameObject);
        }
    }
}
