using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/FerociousHowlSkill")]
public class FerociousHowlSkill : SkillEffect
{
    private Player player;

    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            if (player == null)
            {
                Player player = GameManager.Instance.GetPlayer().GetComponent<Player>();
            }
            float skillRange = player.playerData.skill.skillRange;
            float lastTime = player.playerData.skill.effectLastTime;
            player.SetStunAreaForDuration(lastTime, skillRange);

            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
