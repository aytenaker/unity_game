using game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game {
    public class PlayerLocomotion : MonoBehaviour, IDataPersistence
    {
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector] public Transform myTransform;
        [HideInInspector] public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Fall detection")]
        [SerializeField] float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumFallingDistance = 1f;
        [SerializeField] float groundDirectionRayDistance = 0.3f;
        LayerMask ignoreGround;
        public float MidAirTimer;

        [Header("Stats")]
        [SerializeField] float walkingSpeed = 3;
        [SerializeField] float runningSpeed = 6;
        [SerializeField] float rotationSpeed = 5;
        [SerializeField] float fallSpeed = 150;


        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            playerManager = GetComponent<PlayerManager>();
            cameraObject = Camera.main.transform;
            myTransform = transform;
            animatorHandler.Initialize();

            
            ignoreGround = ~(1 << 8 | 1 << 11);
        }

        public void LoadData(GameData data)
        {
            this.transform.position = data.playerPosition;
        }

        public void SaveData(ref GameData data)
        {
            if (this != null) data.playerPosition = this.transform.position;
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if (animatorHandler.canRotate)
            {
                if (playerManager.focusFlag)
                {
                    if (playerManager.dashFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        if (cameraHandler.currentFocusTarget != null)
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = cameraHandler.currentFocusTarget.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                    }
                }
                else
                {
                    Vector3 targetDir = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount;

                    targetDir = cameraObject.forward * inputHandler.vertical;
                    targetDir += cameraObject.right * inputHandler.horizontal;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero) targetDir = myTransform.forward;

                    float rs = rotationSpeed;
                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                    myTransform.rotation = targetRotation;
                }
            }

            
        }

        public void HandleMovement(float delta)
        {
            if (playerManager.isInteracting) return;
            if (cameraHandler.gameOverMenu.isActive || cameraHandler.finishMenu.isActive)
            {
                moveDirection = Vector3.zero;
                rigidbody.velocity = Vector3.zero;
                animatorHandler.UpdateAnimatorValues(0, 0);
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.y = 0;
            moveDirection.Normalize();
            

            if (playerManager.isWalking || playerManager.focusFlag)
            {
                float speed = walkingSpeed;
                moveDirection *= speed;
            }
            else
            {
                float speed = runningSpeed;
                moveDirection *= speed;
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (playerManager.focusFlag)
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal);
            }
            else
            {
                animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);
            }


        } 

        public void HandleDashing(float delta)
        {
            if (animatorHandler.anim.GetBool("isInteracting")) return;
            if (cameraHandler.gameOverMenu.isActive || cameraHandler.finishMenu.isActive) return;

            if (playerManager.dashFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    animatorHandler.PlayTargetAnimation("Dash_Forward", true);
                    moveDirection.y = 0;
                    Quaternion dashRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = dashRotation;
                    rigidbody.AddForce(transform.forward * 300, ForceMode.VelocityChange);
                } 
                else {
                    animatorHandler.PlayTargetAnimation("Dash_Back", true);
                    rigidbody.AddForce(-transform.forward * 300, ForceMode.VelocityChange);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            playerManager.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (playerManager.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallSpeed);
                rigidbody.AddForce(moveDirection * fallSpeed / 4f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin = origin + dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumFallingDistance, ignoreGround))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                playerManager.isGrounded = true;
                targetPosition.y = tp.y;

                if (playerManager.isInAir)
                {
                    if (MidAirTimer > 0.5f)
                    {
                        animatorHandler.PlayTargetAnimation("Landing", true);
                        MidAirTimer = 0;
                    } else
                    {
                        animatorHandler.PlayTargetAnimation("Empty", false);
                        MidAirTimer = 0;
                    }

                    playerManager.isInAir = false;
                }
            } else
            {
                if (playerManager.isGrounded)
                {
                    playerManager.isGrounded = false;
                } 
                if (playerManager.isInAir == false)
                {
                    if (playerManager.isInteracting == false)
                    {
                        animatorHandler.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * runningSpeed;
                    playerManager.isInAir = true;

                }
            }
            if (playerManager.isInteracting || inputHandler.moveAmount > 0)
            {
                myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime / 0.1f);
            } else
            {
                myTransform.position = targetPosition;
            }
        }

        /*public void HandleJumping()
        {
            if (playerManager.isInteracting) return;
            if (inputHandler.jump_input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                    rigidbody.AddForce(Vector3.up * 120, ForceMode.VelocityChange);
                    rigidbody.AddForce(transform.forward * 80, ForceMode.Acceleration);
                    playerManager.isInAir = true;
                }
                else
                {
                    animatorHandler.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;
                    rigidbody.AddForce(Vector3.up * 120, ForceMode.VelocityChange);
                    playerManager.isInAir = true;
                }
            }
        }*/

        #endregion

    }


}
