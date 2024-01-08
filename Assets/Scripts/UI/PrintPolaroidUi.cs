using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PrintPolaroidUi : MonoBehaviour
{
    [SerializeField]
    private GameEvent _savePicture;

    private UIDocument _document = null;
    private Texture2D _currentImage;
    private PictureData _pictureData;

    // Start is called before the first frame update
    void Start()
    {
        _document = GetComponent<UIDocument>();        
    }

    public void PrintPicture(Component sender, object pictureData)
    {
        if(pictureData is PictureData data)
        {
            _pictureData = data;
            SetImage(data.Picture);
            SetName(data.ObjectName);
        }
    }

    public void SaveImage()
    {
        if (_pictureData != null)
        {
            _savePicture.Raise(this, _pictureData);
            _pictureData = null;
        }
            
    }

    private void SetImage(Texture2D texture)
    {
        _currentImage = texture;

        // Use Unity's main thread execution to update UI
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            var imageElement = _document.rootVisualElement.Q<VisualElement>("Image");
            if (imageElement != null)
            {
                imageElement.style.backgroundImage = _currentImage;
            }
            else
            {
                Debug.LogError("Image element not found in UI hierarchy");
            }
        });
    }

    private void SetName(string name)
    {
        var imageElement = _document.rootVisualElement.Q<Label>("ObjectName");
        if (imageElement != null)
        {
            imageElement.text = name;
        }
        else
        {
            Debug.LogError("Image element not found in UI hierarchy");
        }
    }
}