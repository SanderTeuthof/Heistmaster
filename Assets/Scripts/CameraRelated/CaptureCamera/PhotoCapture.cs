using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    [SerializeField]
    private GameEvent _pictureTaken;

    private Camera _spyCamera;
    private Texture2D _screenCapture;
    private string _name;
    private bool _foundName;

    void Start()
    {
        _screenCapture = new Texture2D(Screen.height, Screen.height, TextureFormat.RGB24, false);   
        _spyCamera = GetComponent<Camera>();
    }

    public void TakePicture()
    {
        _foundName = false;
        StartCoroutine(CapturePhoto());
    }

    IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect((Screen.width-Screen.height)/2f, 0, Screen.height, Screen.height);

        _screenCapture.ReadPixels(regionToRead, 0, 0, false);
        _screenCapture.Apply();

        RaycastHit hit;
        if (RaycastFromCamera(out hit))
        {
            // Check if the object has a NameHolder component
            NameHolder nameHolder = hit.collider.gameObject.GetComponent<NameHolder>();
            if (nameHolder != null)
            {
                _foundName = true;
                _name = nameHolder.ObjectName;
            }

        }

        if (_foundName)
        {
            PictureData pictureData = new PictureData(_screenCapture, _name);

            _pictureTaken.Raise(this, pictureData);

            _foundName = false;
        }
        else
        {
            _name = "No object found";

            PictureData pictureData = new PictureData(_screenCapture, _name);

            _pictureTaken.Raise(this, pictureData);
        }
    }

    private bool RaycastFromCamera(out RaycastHit hit)
    {
        Ray ray = _spyCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        return Physics.Raycast(ray, out hit);
    }
}
