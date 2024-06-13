using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemDBSO")]
public class ItemDBSO : ScriptableObject
{
    [Tooltip("BlueBall, MouseToy, FishToy, FoxTail, Salmon, PaperBox, Necklace, CatBell")]
    public Item[] items; 
}
