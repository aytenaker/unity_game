using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class AnimatorManager : MonoBehaviour
    {
        public Animator anim;
        public bool canRotate;

        public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
        {
            anim.applyRootMotion = isInteracting;
            anim.SetBool("isInteracting", isInteracting);
            anim.SetBool("canRotate", false);
            anim.CrossFade(targetAnimation, 0.2f);
        }
    }
}
