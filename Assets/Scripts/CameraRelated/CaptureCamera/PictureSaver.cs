using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PictureSaver : MonoBehaviour
{
    private List<PictureData> _pictureDataList = new List<PictureData>();


    private static PictureSaver _instance;

    // Singleton pattern to ensure only one instance exists
    public static PictureSaver Instance
    {
        get
        {
            if (_instance == null)
            {
                // If no instance exists, try to find one in the scene
                _instance = FindObjectOfType<PictureSaver>();

                // If no instance is found, create a new one
                if (_instance == null)
                {
                    GameObject go = new GameObject("PictureSaver");
                    _instance = go.AddComponent<PictureSaver>();
                }

                // Mark the GameObject as persistent
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }

    public void AddPictureToList(Component sender, object pictureData)
    {
        if (pictureData is PictureData picture)
        {
            _pictureDataList.Add(picture);
            Debug.Log(_pictureDataList.Count);
        }
    }
}
