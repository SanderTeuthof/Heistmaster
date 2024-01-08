using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShowTextInUI : MonoBehaviour
{
    [SerializeField]
    private string _text;

    private UIDocument _document = null;

    void Start()
    {
        _document = GetComponent<UIDocument>();

        _document.rootVisualElement.Q<Label>().text = _text;
    }

}
