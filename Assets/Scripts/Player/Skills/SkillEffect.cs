using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    None = -1,
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
    public float skillRange;
    public float effectLastTime;
    public Sprite UIImage;

    public float lastExecutionTime;

    protected bool IsCooltimeReady()
    {
        return Time.time >= lastExecutionTime + coolTime;
    }

    protected void UpdateLastExecutionTime()
    {
        lastExecutionTime = Time.time;
    }
}
