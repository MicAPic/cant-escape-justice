using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create ImageRandData", fileName = "ImageRandomData")]
public class ImageRandomData : ScriptableObject
{
    public List<Sprite> images = new List<Sprite>();
}
