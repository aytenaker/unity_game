using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace game
{
    [CreateAssetMenu(menuName = "AI/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 6;
        public float recoveryTime = 2f;

        public float maximumAttackAngle = 45;
        public float minimumAttackAngle = -45;
        public float minimumAttackDistance = 0;
        public float maximumAttackDistance = 2.5f;

    }
}
