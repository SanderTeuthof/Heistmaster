using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVector3VariableInFrontOfObject : MonoBehaviour
{
    public Vector3Reference CameraTarget;
    [SerializeField]
    private FloatReference _cameraTargetDistance;

    private void Awake()
    {
        SetCameraTarget();
    }

    private void Update()
    {
        SetCameraTarget();
    }

    private void SetCameraTarget()
    {
        if (CameraTarget.useConstant)
        {
            CameraTarget.constantValue = transform.position + transform.forward * _cameraTargetDistance.value;
        }
        else
        {
            CameraTarget.variable.value = transform.position + transform.forward * _cameraTargetDistance.value;
        }
    }
}
