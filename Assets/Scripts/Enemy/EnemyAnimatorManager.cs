using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        private void Start()
        {
            anim.logWarnings = false;
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.rigidody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.rigidody.velocity = velocity;
        }
    }
}
