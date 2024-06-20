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
        // Time.realtimeSinceStartup을 사용하여 실제 시간 기준으로 쿨타임을 체크합니다.
        return Time.realtimeSinceStartup >= lastExecutionTime + coolTime;
    }

    protected void UpdateLastExecutionTime()
    {
        // Time.realtimeSinceStartup을 사용하여 실제 시간 기준으로 마지막 실행 시간을 업데이트합니다.
        lastExecutionTime = Time.realtimeSinceStartup;
    }
}
