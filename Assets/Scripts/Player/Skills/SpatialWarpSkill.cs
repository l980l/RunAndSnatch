using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/SpatialWarpSkill")]
public class SpatialWarpSkill : SkillEffect
{
    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            GameManager.Instance.GetPlayer().GetComponent<SpatialWarpAbility>().Teleport();
            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
