using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    ShadowVeil,
    FerociousHowl,
    SpatialWarp,
    ChronoTwist,
    PhantomPassage,
    Max
}

public abstract class SkillEffect : ScriptableObject
{
    public abstract bool ExecuteRole();
    public SkillType skillType;
    public float coolTime;
    public float effectLastTime;
    public float skillRange;
}
