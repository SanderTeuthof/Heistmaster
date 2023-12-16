using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SetVector3VariableInFrontOfObject))]
public class AddCameraTargetToCameraTargets : MonoBehaviour
{
    [SerializeField]
    private GameEvent AddTargetEvent;
    [SerializeField]
    private GameEvent RemoveTargetEvent;

    private Vector3Reference _cameraTarget;

    private void Start()
    {
        SetVector3VariableInFrontOfObject targetHolder = GetComponent<SetVector3VariableInFrontOfObject>();
        _cameraTarget = targetHolder.CameraTarget;
    }
    public void AddTarget()
    {
        AddTargetEvent.Raise(this, _cameraTarget);
    }

    public void RemoveTarget()
    {
        RemoveTargetEvent.Raise(this, _cameraTarget);
    }
}
