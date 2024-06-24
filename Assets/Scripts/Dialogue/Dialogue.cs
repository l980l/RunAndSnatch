using System.Collections.Generic;
using UnityEngine;

public enum LanguageType
{
    En,
    Kr,
}

[CreateAssetMenu(menuName = "ScriptableObject/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string characterNameEN;
    public string characterNameKR;
    public string[] sentencesEN;
    public string[] sentencesKR;
}