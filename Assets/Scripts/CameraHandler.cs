using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game
{
    public class CameraHandler : MonoBehaviour
    {
        PlayerManager playerManager;

        [SerializeField] public GameOverMenu gameOverMenu;
        [SerializeField] public FinishMenu finishMenu;

        public Transform targetTransform;
        public Transform cameraTransform;
        public Transform cameraPivotTransform;
        private Transform myTransform;
        private Vector3 cameraTransformPosition;
        private LayerMask ignoreLayers;
        private LayerMask environmentLayer;
        private Vector3 cameraFollowVelocity = Vector3.zero;

        public float lookSpeed = 0.03f;
        public float followSpeed = 0.3f;
        public float pivotSpeed = 0.01f;

        private float targetPosition;
        private float defaultPosition;
        private float lookAngle;
        private float pivotAngle;
        public float minimumPivotAngle = -60;
        public float maximumPivotAngle = 80;

        public float cameraSphereRadius = 0.2f;
        public float cameraCollisionOffset = 0.2f;
        public float minimumCollisionOffset = 0.2f;
        public float focusedPivotPosition = 3f;
        public float unfocusedPivotPosition = 1.65f;

        List<CharacterManager> availableTargets = new List<CharacterManager>();
        public float maximumFocusDistance = 30;
        public Transform nearestFocusTarget;
        public Transform currentFocusTarget;
        public Transform leftFocusTarget;
        public Transform rightFocusTarget;

        private void Awake()
        {
            myTransform = transform;
            defaultPosition = cameraTransform.localPosition.z;
            ignoreLayers = ~(1 << 9 | 1 << 10 | 1 << 12);
            targetTransform = FindObjectOfType<PlayerManager>().transform;
            playerManager = FindObjectOfType<PlayerManager>();
            gameOverMenu = FindObjectOfType<GameOverMenu>();
            finishMenu = FindObjectOfType<FinishMenu>();
        }

        private void Start()
        {
            environmentLayer = LayerMask.NameToLayer("Environment");
        }

        public void FollowTarget(float delta)
        {
            Vector3 targetPosition = Vector3.SmoothDamp(myTransform.position, targetTransform.position, ref cameraFollowVelocity, delta / followSpeed);
            myTransform.position = targetPosition;
            HandleCameraCollisions(delta);
        }

        public void HandleCameraRotation(float delta, float mouseXInput, float mouseYInput)
        {
            if (gameOverMenu.isActive || finishMenu.isActive) return;
            if (playerManager.focusFlag == false && currentFocusTarget == null)
            {
                lookAngle += (mouseXInput * lookSpeed) / delta;
                pivotAngle -= (mouseYInput * pivotSpeed) / delta;
                pivotAngle = Mathf.Clamp(pivotAngle, minimumPivotAngle, maximumPivotAngle);

                Vector3 rotation = Vector3.zero;
                rotation.y = lookAngle;
                Quaternion targetRotation = Quaternion.Euler(rotation);
                myTransform.rotation = targetRotation;

                rotation = Vector3.zero;
                rotation.x = pivotAngle;

                targetRotation = Quaternion.Euler(rotation);
                cameraPivotTransform.localRotation = targetRotation;
            }
            else
            {
                float velocity = 0;

                Vector3 dir = currentFocusTarget.position - transform.position;
                dir.y = 0;
                dir.Normalize();
                

                Quaternion targetRotation = Quaternion.LookRotation(dir);
                transform.rotation = targetRotation;

                dir = currentFocusTarget.position - cameraPivotTransform.position;
                dir.Normalize();

                targetRotation = Quaternion.LookRotation(dir);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;
                cameraPivotTransform.localEulerAngles = eulerAngle;
            }
        }

        private void HandleCameraCollisions(float delta)
        {
            targetPosition = defaultPosition;
            RaycastHit hit;
            Vector3 direction = cameraTransform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast
                (cameraPivotTransform.position, cameraSphereRadius, direction, out hit, Mathf.Abs(targetPosition), ignoreLayers))
            {
                float dist = Vector3.Distance(cameraPivotTransform.position, hit.point);
                targetPosition = -(dist - cameraCollisionOffset);
            }

            if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
            {
                targetPosition = -minimumCollisionOffset;
            }

            cameraTransformPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, delta / 0.2f);
            cameraTransform.localPosition = cameraTransformPosition;
        }

        public void HandleFocus()
        {
            availableTargets.Clear();

            float shortestDistance = Mathf.Infinity;
            float shortestDistanceToLeftTarget = Mathf.Infinity;
            float shortestDistanceToRightTarget = Mathf.Infinity;


            Collider[] colliders = Physics.OverlapSphere(targetTransform.position, 30);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterManager character = colliders[i].GetComponent<CharacterManager>();

                if (character != null)
                {
                    Vector3 focusTargetDirection = character.transform.position - targetTransform.position;
                    float distanceFromTarget = Vector3.Distance(targetTransform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(focusTargetDirection, cameraTransform.forward);
                    RaycastHit hit;

                    if (character.transform.root != targetTransform.transform.root 
                        && viewableAngle > -90 && viewableAngle < 90
                        && distanceFromTarget <= maximumFocusDistance)
                    {
                        if (Physics.Linecast(playerManager.focusTransform.position, character.focusTransform.position, out hit))
                        {
                            if (hit.transform.gameObject.layer == environmentLayer)
                            {
                                // don't focus, obstacle ahead
                            } else
                            {
                                availableTargets.Add(character);
                            }
                        }
                    }
                }
            }

            for (int k = 0; k < availableTargets.Count; k++)
            {
                float distanceFromTarget = Vector3.Distance(targetTransform.position, availableTargets[k].transform.position);

                if (distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestFocusTarget = availableTargets[k].focusTransform;
                }

                if (playerManager.focusFlag)
                {
                    Vector3 relativeEnemyPosition = currentFocusTarget.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceToLeftTarget = currentFocusTarget.transform.position.x - availableTargets[k].transform.position.x;
                    var distanceToRightTarget = currentFocusTarget.transform.position.x + availableTargets[k].transform.position.x;

                    if (relativeEnemyPosition.x < 0.0 && distanceToLeftTarget < shortestDistanceToLeftTarget)
                    {
                        shortestDistanceToLeftTarget = distanceToLeftTarget;
                        leftFocusTarget = availableTargets[k].focusTransform;
                    }
                    if (relativeEnemyPosition.x > 0.0 && distanceToRightTarget < shortestDistanceToRightTarget)
                    {
                        shortestDistanceToRightTarget = distanceToRightTarget;
                        rightFocusTarget = availableTargets[k].focusTransform;
                    }
                }
            }
        }

        public void ClearFocusedTargets()
        {
            availableTargets.Clear();
            nearestFocusTarget = null;
            currentFocusTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newFocusedPosition = new Vector3(0, focusedPivotPosition, 0);
            Vector3 newUnfocusedPosition = new Vector3(0, unfocusedPivotPosition, 0);

            if (currentFocusTarget != null)
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newFocusedPosition, ref velocity, Time.deltaTime);
            } else
            {
                cameraPivotTransform.transform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.transform.localPosition, newUnfocusedPosition,ref velocity, Time.deltaTime / 1f);
            }

        }
    }
}
