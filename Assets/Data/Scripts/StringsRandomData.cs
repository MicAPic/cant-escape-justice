using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create StringsRandomData", fileName = "StringsRandomData")]
public class StringsRandomData : ScriptableObject
{
    public List<string> strings = new List<string>();
}

