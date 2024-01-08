using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchToSpyCamera : MonoBehaviour
{
    [SerializeField]
    private Camera _spyCamera;

    private Camera _cameraMain;

    private void Awake()
    {
        _cameraMain = Camera.main;
    }

    public void SetToSpyCamera()
    {
        _cameraMain.enabled = false;
        _spyCamera.enabled = true;
    }

    public void SetToMainCamera()
    {
        _cameraMain.enabled = true;
        _spyCamera.enabled = false;
    }
}
