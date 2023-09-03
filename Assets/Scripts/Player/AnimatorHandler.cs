using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class AnimatorHandler : AnimatorManager
    {
        PlayerManager playerManager;
        PlayerLocomotion playerLocomotion;

        int vertical, horizontal;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            anim = GetComponent<Animator>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
            anim.logWarnings = false;
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement)
        {
            #region Vertical
            float v = 0;
            

            if (verticalMovement > 0 && verticalMovement < 0.55f) {
                v = 0.5f;
            } else if (verticalMovement > 0.55f) {
                v = 1;
            } else if (verticalMovement < 0 && verticalMovement < -0.55f) {
                v = -0.5f;
            } else if (verticalMovement < -0.55f) {
                v = -1;
            } else {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement < -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion

            if (playerManager.isWalking || playerManager.focusFlag)
            {
                if (verticalMovement == 0 && horizontalMovement == 0)
                {
                    h = 0;
                    v = 0;
                }
                else
                {
                    h = horizontalMovement;
                    v = 0.5f;
                }
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canCombo", false);
        }

        public void EnableInvincibility()
        {
            anim.SetBool("isInvincible", true);
        }

        public void DisableInvincibility()
        {
            anim.SetBool("isInvincible", false);
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;
        }
    }
}