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
    private Vector3 _previouMovement = Vector3.zero;

    private bool _grounded;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _camera = Camera.main;
    }

    private void Update()
    {
        CheckIfGrounded();

        CalculateMovementInput();
        RotateCharacterToWalkDirection();
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

    private void MovePlayer()
    {
        _characterController.Move(_movement * Time.deltaTime);
        _previouMovement = _movement;
        _movement = Vector3.zero;
    }

    private void CalculateMovementInput()
    {
        Vector3 cameraForward = _camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        Vector3 cameraSidewards = Vector3.Cross(cameraForward, Vector3.up).normalized;

        float horizontalInput = -Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        _movement += (cameraForward * verticalInput + cameraSidewards * horizontalInput) * _movementSpeed.value;        
    }

    private void OnTriggerStay(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Collider>().attachedRigidbody;

        if (otherRigidbody != null)
        {
            Vector3 directionToOther = Vector3.Normalize(other.transform.position - transform.position);
            Vector3 force = directionToOther * _previouMovement.magnitude * _pushForce.value;

            otherRigidbody.AddForce(force, ForceMode.Force);
        }
    }
}
