using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class ChaseState : State
    {
        public CombatStanceState combatStanceState;
        public int movementSpeed;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerformingAction || enemyManager.isInteracting) return this;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimatorManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                enemyManager.transform.position = Vector3.MoveTowards(enemyManager.transform.position, enemyManager.nav.transform.position, movementSpeed * Time.deltaTime);
            }

            HandleRotation(enemyManager);

            if (distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            } 
        }
        public void HandleRotation(EnemyManager enemyManager)
        {
            Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            direction.y = 0;
            direction.Normalize();

            if (direction == Vector3.zero)
            {
                direction = enemyManager.transform.forward;
            }

            enemyManager.nav.enabled = true;
            enemyManager.nav.SetDestination(enemyManager.currentTarget.transform.position);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed / Time.deltaTime);
        }
    }
}
