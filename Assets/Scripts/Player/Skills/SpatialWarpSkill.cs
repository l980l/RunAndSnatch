using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/SpatialWarpSkill")]
public class SpatialWarpSkill : SkillEffect
{
    private SpatialWarpAbility spatialWarpAbility;

    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            if (spatialWarpAbility == null)
            {
                spatialWarpAbility = GameManager.Instance.GetPlayer().GetComponent<SpatialWarpAbility>();
            }
            spatialWarpAbility.Teleport();
            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
