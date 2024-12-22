using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCrate : ScriptableObject
{
    public string Barcode = "THISWILLAUTOCREATE";
    public string Title;
    public string Description;
    public bool Redacted = false;
    public string[] Tags;

    public string LevelSceneName;

}
