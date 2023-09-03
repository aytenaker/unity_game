using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 20;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;

        public ChaseState chaseState;


        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (isSleeping && enemyManager.isInteracting == false) {
                enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);
            }

            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    Vector3 targetDirection = characterStats.transform.position - enemyManager.transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle 
                        && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        isSleeping = false;
                        enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }

            if (enemyManager.currentTarget != null)
            {
                return chaseState;
            } else
            {
                return this;
            }
        }
    }
}
