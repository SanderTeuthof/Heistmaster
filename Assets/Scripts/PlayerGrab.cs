using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrab : MonoBehaviour
{
    [SerializeField]
    private FloatReference _yOffset;
    [SerializeField]
    private FloatReference _zOffset;
    [SerializeField]
    private FloatReference _grabDistance;
    [SerializeField]
    private FloatReference _grabForce;
    [SerializeField]
    private FloatReference _grabSphereRadius;

    private bool _isGrabbing;
    private Rigidbody _grabbedRB;
    private Vector3 _targetPosition;

    private void FixedUpdate()
    {
        if (_isGrabbing)
        {
            _targetPosition = transform.position + transform.forward * _zOffset.value;
            _targetPosition.y += _yOffset.value;

            // Check if object is far away from the target point, if it is, stop the grab
            if (Vector3.Distance(_targetPosition, _grabbedRB.position) > _grabDistance.value)
            {
                LetGoOfGrab();
                return;
            }

            if(Vector3.Distance(_targetPosition, _grabbedRB.position) > _grabSphereRadius.value)
            {
                Vector3 forceDirection = (_targetPosition - _grabbedRB.position).normalized;
                _grabbedRB.AddForce(forceDirection * _grabForce.value*10);
            }
            else
            {
                // Drag held Rigidbody in front of Character controller using physics            
                Vector3 forceDirection = (_targetPosition - _grabbedRB.position).normalized;
                _grabbedRB.AddForce(forceDirection * _grabForce.value);
            }
            
            
        }        
    }

    public void Grab()
    {
        if (_isGrabbing)
        {
            LetGoOfGrab();
            return;
        }
        CheckInFrontToGrab();
    }

    private void CheckInFrontToGrab()
    {
        RaycastHit hit;

        // Cast a ray in front of the player
        if (Physics.Raycast(transform.position, transform.forward, out hit, _grabDistance.value))
        {
            Debug.DrawRay(transform.position, transform.forward * _grabDistance.value, Color.green);

            Rigidbody hitRB = hit.collider.GetComponent<Rigidbody>();

            if (hitRB != null)
            {
                // Grab the Rigidbody
                _grabbedRB = hitRB;
                _isGrabbing = true;
            }
        }
    }

    private void LetGoOfGrab()
    {
        if (_grabbedRB != null)
        {
            _grabbedRB = null;
            _isGrabbing = false;
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a representation of the grab distance in the Scene view
        Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position + transform.forward * _grabDistance.value, 0.1f);

        _targetPosition = transform.position + transform.forward * _zOffset.value;
        _targetPosition.y += _yOffset.value;

        Gizmos.DrawWireSphere(_targetPosition, 0.1f);
    }
}
