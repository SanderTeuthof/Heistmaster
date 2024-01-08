using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotationMouse : MonoBehaviour
{
    [SerializeField] 
    private FloatReference _sensitivity;
    [SerializeField] 
    private FloatReference _yRotationLimit;
    [SerializeField]
    private FloatReference _xRotationLimit;

    private Vector2 _rotation = Vector2.zero;
    private const string _xAxis = "Mouse X";
    private const string _yAxis = "Mouse Y";

    void Update()
    {
        _rotation.x += Input.GetAxis(_xAxis) * _sensitivity.value;

        if (_xRotationLimit.value != 0)
            _rotation.x = Mathf.Clamp(_rotation.x, -_xRotationLimit.value, _xRotationLimit.value);

        _rotation.y += Input.GetAxis(_yAxis) * _sensitivity.value;
        _rotation.y = Mathf.Clamp(_rotation.y, -_yRotationLimit.value, _yRotationLimit.value);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        transform.rotation = xQuat * yQuat;
    }
}
