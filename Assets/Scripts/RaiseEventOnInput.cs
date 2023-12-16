using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaiseEventOnInput : MonoBehaviour
{
    [SerializeField]
    private string _inputToTriggerEvent;
    [SerializeField]
    private GameEvent ButtonPressed;
    [SerializeField] 
    private GameEvent ButtonReleased;

    private bool _isButtonDown = false;

    private void Update()
    {
        bool isButtonPressed = Input.GetButton(_inputToTriggerEvent);

        if (isButtonPressed && !_isButtonDown)
        {
            ButtonPressed?.Raise();

            _isButtonDown = true;
        }

        else if (!isButtonPressed && _isButtonDown)
        {
            ButtonReleased?.Raise();

            _isButtonDown = false;
        }
    }
}
