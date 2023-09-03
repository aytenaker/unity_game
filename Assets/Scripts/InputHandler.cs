using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool alt_input;
        public bool attack_input;
        public bool special_input;
        public bool jump_input;
        public bool focus_input;
        public bool switch_target_to_left_input;
        public bool switch_target_to_right_input;

        public Animator animator;
        Controls inputActions;
        PlayerLocomotion playerLocomotion;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            playerLocomotion = GetComponentInChildren<PlayerLocomotion>();
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
        }

        private void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new Controls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.Walking.performed += i => alt_input = true;
                inputActions.PlayerActions.Walking.canceled += i => alt_input = false;
                inputActions.PlayerActions.Focus.performed += i => focus_input = true;
                inputActions.PlayerMovement.SwitchTargetToLeft.performed += i => switch_target_to_left_input = true;
                inputActions.PlayerMovement.SwitchTargetToRight.performed += i => switch_target_to_right_input = true;
                inputActions.PlayerActions.Attack.performed += i => attack_input = true;
                inputActions.PlayerActions.SpecialAttack.performed += i => special_input = true;
                inputActions.PlayerActions.Jump.performed += inputActions => jump_input = true;
            }
            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            DashInput(delta);
            WalkInput(delta);
            AttackInput(delta);
            FocusInput();
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void DashInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Dash.phase == UnityEngine.InputSystem.InputActionPhase.Performed;

            if (b_Input)
            {
                playerManager.dashFlag = true;
            }
        }

        private void WalkInput(float delta)
        {
            if (alt_input && moveAmount > 0.5f)
            {
                playerManager.isWalking = true;
            }
            else
            {
                playerManager.isWalking = false;
            }
        }

        private void AttackInput(float delta)
        {

            if (attack_input)
            {
                if (playerManager.canCombo)
                {
                    playerManager.comboFlag = true;
                    playerAttacker.HandleCombo(playerInventory.rightWeapon);
                    playerManager.comboFlag = false;
                } else
                {
                    playerAttacker.HandleAttack(playerInventory.rightWeapon);
                }
                
            }

            if (special_input)
            {
                if (playerManager.canCombo)
                {
                    playerManager.comboFlag = true;
                    playerAttacker.HandleSpecialCombo(playerInventory.rightWeapon);
                    playerManager.comboFlag = false;
                }
                else
                {
                    playerAttacker.HandleSpecialAttack(playerInventory.rightWeapon);
                }

            }
        }

        private void FocusInput()
        {
            if (focus_input && playerManager.focusFlag == false)
            {
                focus_input = false;
                playerManager.cameraHandler.HandleFocus();
                if (playerManager.cameraHandler.nearestFocusTarget != null)
                {
                    playerManager.cameraHandler.currentFocusTarget = playerManager.cameraHandler.nearestFocusTarget;
                    playerManager.focusFlag = true;
                    animator.SetBool("isFocused", playerManager.focusFlag);
                }
            } else if (focus_input && playerManager.focusFlag)
            {
                focus_input = false;
                playerManager.focusFlag = false;
                animator.SetBool("isFocused", playerManager.focusFlag);
                playerManager.cameraHandler.ClearFocusedTargets();
            }

            if (playerManager.focusFlag && switch_target_to_left_input)
            {
                switch_target_to_left_input = false;
                playerManager.cameraHandler.HandleFocus();
                if (playerManager.cameraHandler.leftFocusTarget != null)
                {
                    playerManager.cameraHandler.currentFocusTarget = playerManager.cameraHandler.leftFocusTarget;
                }
            }
            if (playerManager.focusFlag && switch_target_to_right_input)
            {
                switch_target_to_right_input = false;
                playerManager.cameraHandler.HandleFocus();
                if (playerManager.cameraHandler.rightFocusTarget != null)
                {
                    playerManager.cameraHandler.currentFocusTarget = playerManager.cameraHandler.rightFocusTarget;
                }
            }

            playerManager.cameraHandler.SetCameraHeight();
        }

    }
}
