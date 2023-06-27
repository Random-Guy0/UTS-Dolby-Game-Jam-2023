using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private PlayerInput inputComponent;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject UI;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float lookSensitivity = 1.0f;

    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.3f;
    
    private Vector3 velocity;
    private Vector2 input;
    private Vector2 mouseLook;

    private void Start()
    {
        if (IsOwner)
        {
            playerCamera.SetActive(true);
            playerCamera.tag = "MainCamera";
            inputComponent.enabled = true;
            UI.SetActive(true);
        }
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector3 forwardVelocity = transform.forward * input.y;
        Vector3 rightVelocity = transform.right * input.x;

        float currentY = velocity.y;
        velocity = forwardVelocity + rightVelocity;
        velocity *= speed;
        velocity.y = currentY;
        
        if (characterController.isGrounded)
        {
            if (animator.isActiveAndEnabled)
            {
                animator.SetBool("Grounded", true);
            }

            if (velocity.y < 0f)
            {
                velocity.y = 0f;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (animator.isActiveAndEnabled)
        {
            animator.SetFloat("MotionSpeed", velocity.magnitude);
            animator.SetFloat("Speed", speed);
        }
    }

    private void Look()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += mouseLook.x;
        transform.rotation = Quaternion.Euler(rotation);

        Vector3 cameraRotation = playerCamera.transform.rotation.eulerAngles;
        cameraRotation.x += mouseLook.y;
        if (cameraRotation.x > 45f && cameraRotation.x < 180f)
        {
            cameraRotation.x = 45f;
        }
        else if (cameraRotation.x < 315f && cameraRotation.x >= 180f)
        {
            cameraRotation.x = 315f;
        }
        
        playerCamera.transform.rotation = Quaternion.Euler(cameraRotation);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookInput = context.ReadValue<Vector2>();
        mouseLook.x = lookInput.x * lookSensitivity;
        mouseLook.y = -lookInput.y * lookSensitivity;
    }
    
    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(characterController.center), FootstepAudioVolume);
            }
        }
    }
}
