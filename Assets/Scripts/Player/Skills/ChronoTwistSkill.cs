using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/ChronoTwistSkill")]
public class ChronoTwistSkill : SkillEffect
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
            float lastTime = player.playerData.skill.effectLastTime;
            player.GetComponent<ChronoTwistAbility>().UseChronoTwist(lastTime);
            
            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
