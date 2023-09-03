using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class EndPoint : MonoBehaviour
    {
        public Collider collider;
        public ParticleSystem glow;
        public FinishMenu finishMenu;
        public GameObject light;

        private void Awake()
        {
            collider = GetComponent<CapsuleCollider>();
            glow = GetComponentInChildren<ParticleSystem>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                finishMenu.Finish();
            }
        }
    }
}
