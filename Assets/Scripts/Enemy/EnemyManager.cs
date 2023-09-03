using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace game
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyStats enemyStats;

        public bool isPerformingAction;
        public bool isInteracting;

        CapsuleCollider capsuleCollider;
        public Rigidbody rigidody;
        public NavMeshAgent nav;
        public State currentState;
        public CharacterStats currentTarget;

        public float rotationSpeed = 10f;
        public float maximumAttackRange = 2f;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        public float minimumDetectionAngle = -180;
        public float maximumDetectionAngle = 180;
        public float currentRecoveryTime = 0;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            capsuleCollider = GetComponent<CapsuleCollider>();
            rigidody = GetComponentInParent<Rigidbody>();
            nav = GetComponentInChildren<NavMeshAgent>();
            nav.enabled = false;
        }


        private void Start()
        {
            rigidody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTime();
            HandleStates();

            isInteracting = enemyAnimatorManager.anim.GetBool("isInteracting");

            if (enemyStats.isDead)
            {
                currentState = null;
                capsuleCollider.enabled = false;
            }

        }

        private void HandleStates()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchState(nextState);
                }
            }

        }

        private void SwitchState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }

        }
    }
}
