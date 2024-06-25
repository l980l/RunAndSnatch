using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemDBSO")]
public class ItemDBSO : ScriptableObject
{
    [Tooltip("BlueBall, MouseToy, FishToy, DogtailGrass, Salmon, PaperBox, Necklace, CatBell")]
    public Item[] items; 
}
