using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace game
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        PlayerLocomotion playerLocomotion;
        AnimatorHandler animatorHandler;
        public CameraHandler cameraHandler;

        public bool canCombo;
        public bool isInvincible;
        public bool isInteracting;
        public bool isWalking;
        public bool focusFlag;
        public bool dashFlag;
        public bool comboFlag;
        public bool isInAir;
        public bool isGrounded;


        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            inputHandler = GetComponent<InputHandler>();
            anim = GetComponentInChildren<Animator>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();

            isGrounded = true;
        }

        void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            isInvincible = anim.GetBool("isInvincible");
            canCombo = anim.GetBool("canCombo");
            anim.SetBool("isInAir", isInAir);
            animatorHandler.canRotate = anim.GetBool("canRotate");

            inputHandler.TickInput(delta);
            playerLocomotion.HandleDashing(delta);

            if (focusFlag && cameraHandler.currentFocusTarget.gameObject.activeSelf == false)
            {
                focusFlag = false;
                cameraHandler.ClearFocusedTargets();
            }
        }

        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRotation(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
        }

        private void LateUpdate()
        {
            dashFlag = false;
            inputHandler.attack_input = false;
            inputHandler.special_input = false;
            inputHandler.jump_input = false;

            float delta = Time.deltaTime;

            if (isInAir) playerLocomotion.MidAirTimer += Time.deltaTime;
        }

    }
}
