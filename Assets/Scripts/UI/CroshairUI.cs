using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CroshairUI : MonoBehaviour
{
    private UIDocument _document = null;

    void Start()
    {
        _document = GetComponent<UIDocument>();
        Hide();
    }

    public void Show()
    {
        _document.rootVisualElement.visible = true;
    }
    public void Hide()
    {
        _document.rootVisualElement.visible = false;
    }
}
