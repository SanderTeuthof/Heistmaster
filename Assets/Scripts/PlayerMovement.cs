using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private FloatReference _movementSpeed;
    [SerializeField]
    private FloatReference _rotationSpeed;
    [SerializeField]
    private FloatReference _pushForce;

    private CharacterController _characterController;
    private Camera _camera;

    private Vector3 _movement = Vector3.zero;

    private bool _grounded;
    private bool _usingController;

    private const string _horizontalMovement = "Horizontal";
    private const string _verticalMovement = "Vertical";

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
        _usingController = false;
    }

    private void Update()
    {
        CheckIfGrounded();

        CalculateMovementInput();
        RotateCharacterTowardsCursor();
        //RotateCharacterToWalkDirection();
        ApplyGraity();

        MovePlayer();
    }

    private void CheckIfGrounded()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        float rayLength = _characterController.height * 0.5f + 0.1f;

        _grounded = Physics.Raycast(ray, rayLength);
    }

    private void ApplyGraity()
    {
        Vector3 gravity = Physics.gravity;

        if (!_grounded)
        {
            _movement += gravity;
        }
        else
        {
            _movement.y -= 1 * Time.deltaTime;
        }
    }

    private void RotateCharacterToWalkDirection()
    {
        if (_movement != Vector3.zero)
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(_movement.normalized, Vector3.up);

            // Smoothly interpolate between the current rotation and the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.value * Time.deltaTime);
        }
    }

    private void RotateCharacterTowardsCursor()
    {
        if (_usingController)
        {
            // Use controller right stick input for rotation
            float horizontalInput = Input.GetAxis("LookX");
            float verticalInput = Input.GetAxis("LookY");

            Vector3 controllerDirection = new Vector3(horizontalInput, 0f, verticalInput);
            if (controllerDirection.magnitude > 0.1f)
            {
                Debug.Log(controllerDirection);
                // Calculate the target rotation based on the controller input
                Quaternion targetRotation = Quaternion.LookRotation(controllerDirection, Vector3.up);

                // Smoothly interpolate between the current rotation and the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.value * Time.deltaTime);
            }
        }
        else
        {
            // Use mouse input for rotation
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                targetPosition.y = transform.position.y; // Keep the same y-coordinate as the player

                // Calculate the target rotation based on the direction towards the cursor
                Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position, Vector3.up);

                // Smoothly interpolate between the current rotation and the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed.value * Time.deltaTime);
            }
        }
    }

    private bool IsUsingController()
    {
        string[] joystickNames = Input.GetJoystickNames();

        for (int i = 0; i < joystickNames.Length; i++)
        {
            if (!string.IsNullOrEmpty(joystickNames[i]))
            {
                // Joystick is connected
                return true;
            }
        }

        return false;
    }

    private void MovePlayer()
    {
        _characterController.Move(_movement * Time.deltaTime);
        _movement = Vector3.zero;
    }

    private void CalculateMovementInput()
    {
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        Vector3 cameraSidewards = Vector3.Cross(cameraForward, Vector3.up).normalized;

        float horizontalInput = -Input.GetAxis(_horizontalMovement);
        float verticalInput = Input.GetAxis(_verticalMovement);

        _movement += (cameraForward * verticalInput + cameraSidewards * horizontalInput) * _movementSpeed.value;        
    }

    /*private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Collider>().attachedRigidbody;

        if (otherRigidbody != null)
        {
            Vector3 directionToOther = Vector3.Normalize(other.transform.position - transform.position);
            Vector3 force = directionToOther * _previouMovement.magnitude * _pushForce.value;

            otherRigidbody.AddForce(force, ForceMode.Force);
        }
    }*/

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        if(rb != null)
        {
            Vector3 forceDirection = hit.transform.position - transform.position; 
            forceDirection.y = 0;
            forceDirection = forceDirection.normalized;

            rb.AddForceAtPosition(forceDirection * _pushForce.value, transform.position, ForceMode.Impulse);
        }
    }

    public void SwitchControllerInput()
    {
        _usingController = !_usingController;
    }
}
