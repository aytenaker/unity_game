using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    [CreateAssetMenu(menuName = "Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;

        public string Attack_01;
        public string Attack_02;
        public string Attack_03;
        public string Attack_04;
        public string Attack_05;
        public string SpecialAttack_01;
        public string SpecialAttack_02;
        public string SpecialAttack_03;
    }
}
