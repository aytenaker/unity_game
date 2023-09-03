using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public ChaseState chaseState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 0);
            }

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget < enemyManager.maximumAttackRange)
            {
                return attackState;
            } 
            else if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return chaseState;
            } 
            else
            {
                return this;
            }
        }
    }
}
