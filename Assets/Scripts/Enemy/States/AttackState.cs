using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class AttackState : State
    {
        public CombatStanceState combatStanceState;

        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerformingAction) return combatStanceState;

            if (currentAttack == null)
            {
                GetAttack(enemyManager);
            }
            else
            {
                enemyManager.isPerformingAction = true;
                enemyManager.currentRecoveryTime = currentAttack.recoveryTime;
                enemyAnimatorManager.PlayTargetAnimation(currentAttack.AnimationName, true);
                currentAttack = null;
            }

            if (enemyManager.currentRecoveryTime <= 0f) return combatStanceState;
            return this;
        }

        private void GetAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumAttackDistance
                    && distanceFromTarget >= enemyAttackAction.minimumAttackDistance)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore = enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int tempScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumAttackDistance
                    && distanceFromTarget >= enemyAttackAction.minimumAttackDistance)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (currentAttack != null) return;

                        tempScore += enemyAttackAction.attackScore;

                        if (tempScore > randomValue)
                        {
                            currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
    }
}
