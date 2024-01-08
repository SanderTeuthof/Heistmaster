using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureData
{
    public PictureData(Texture2D picture, string objectName)
    {
        Picture = picture;
        ObjectName = objectName;
        _timeTaken = DateTime.Now;
        TimeTakenAsInt = (int)(_timeTaken - new DateTime(1970, 1, 1)).TotalSeconds;
    }

    public Texture2D Picture { get; }
    public string ObjectName { get; }

    private DateTime _timeTaken;

    public int TimeTakenAsInt { get; }

    public string FilePath { get; set; }
}
