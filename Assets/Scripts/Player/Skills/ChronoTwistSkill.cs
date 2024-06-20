using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/ChronoTwistSkill")]
public class ChronoTwistSkill : SkillEffect
{
    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            // 스킬 실행 로직
            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
