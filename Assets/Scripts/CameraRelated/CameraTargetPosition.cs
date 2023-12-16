using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetPosition : MonoBehaviour
{
    [SerializeField]
    private List<Vector3Reference> _targetLocations = new List<Vector3Reference>();

    [SerializeField]
    private FloatReference _lerpSpeed;

    private Vector3 _averageTarget;

    private void Start()
    {
        CalculateAverageTarget();
        SnapToTargetPosition();
    }

    private void SnapToTargetPosition()
    {
        transform.position = _averageTarget;
    }

    private void Update()
    {
        CalculateAverageTarget();
        LerpTowardsTarget();
    }

    private void CalculateAverageTarget()
    {
        if (_targetLocations.Count == 0)
        {
            // No targets in the list, set average target to current position
            _averageTarget = transform.position;
            return;
        }

        // Initialize the sum of target positions
        Vector3 sum = Vector3.zero;

        // Sum up the positions of all targets
        foreach (Vector3Reference targetLocation in _targetLocations)
        {
            sum += targetLocation.Value;
        }

        // Calculate the average target position
        _averageTarget = sum / _targetLocations.Count;
    }

    private void LerpTowardsTarget()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, _averageTarget, _lerpSpeed.value * Time.deltaTime);

        transform.position = newPosition;
    }

    public void AddCameraTarget(Component sender, object data)
    {
        Vector3Reference vector3Ref = data as Vector3Reference;

        if (vector3Ref != null)
        {
            _targetLocations.Add(vector3Ref);
        }        
    }

    public void RemoveCameraTarget(Component sender, object data)
    {
        Vector3Reference vector3Ref = data as Vector3Reference;

        if (vector3Ref != null)
        {
            _targetLocations.Remove(vector3Ref);
        }
    }
}
