using game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace game
{
    public class GameManager : MonoBehaviour
    {
        EnemyStats enemyStats;
        public Animator previousDoor;
        public Animator[] doorsToOpen;
        public GameObject previousCheckpoint;
        public GameObject nextCheckpoint;

        public List<GameObject> enemies = new List<GameObject>();
        public List<GameObject> enemiesToActivate = new List<GameObject>();

        public float secondsToWait;

        private void Update()
        {
            CheckForDeadEnemies();
            if (enemies.Count == 0)
            {
                for (int i = 0; i < doorsToOpen.Length; i++)
                {
                    doorsToOpen[i].Play("Open");
                }
                if (previousCheckpoint != null)
                {
                    Destroy(previousCheckpoint.gameObject);
                }
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(ActivateEnemies(secondsToWait));
                if (previousDoor != null)
                {
                    previousDoor.Play("Close");
                }
 
            }
        }

        public void CheckForDeadEnemies()
        {
            for (int i = enemies.Count - 1; i > -1; i--)
            {
                enemyStats = enemies[i].GetComponent<EnemyStats>();
                if (enemyStats.isDead)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        private void OnDestroy()
        {
            if (nextCheckpoint != null)
            {
                CheckPoint checkPointComponent = nextCheckpoint.GetComponent<CheckPoint>();
                EndPoint endPointComponent = nextCheckpoint.GetComponent<EndPoint>();
                if (checkPointComponent != null)
                {
                    checkPointComponent.collider.enabled = true;
                    checkPointComponent.GetComponentInChildren<ParticleSystem>().Play();
                    checkPointComponent.light.SetActive(true);
                }
                else if (endPointComponent != null)
                {
                    endPointComponent.collider.enabled = true;
                    endPointComponent.GetComponentInChildren<ParticleSystem>().Play();
                    endPointComponent.light.SetActive(true);
                }
            }
        }

        IEnumerator ActivateEnemies(float seconds)
        {
            for (int i = 0; i < enemiesToActivate.Count; i++)
            {
                yield return new WaitForSeconds(seconds);
                enemiesToActivate[i].GetComponent<EnemyManager>().enabled = true;
                enemiesToActivate[i].GetComponent<CapsuleCollider>().enabled = true;
            }
        }

    }
}
