using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : ScriptableObject
{
    public string Barcode = "THISWILLAUTOCREATE";
    public string Title;
    public string Description;
    public bool Redacted = false;
    public string[] Tags;
    public Texture2D CrateLogo;

    public GameObject CrateSpawnable;  // Reference to the prefab of the crate
  
}
