using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : ScriptableObject
{
    public abstract bool ExecuteRole();
    public float value1;
    public float value2;
}
