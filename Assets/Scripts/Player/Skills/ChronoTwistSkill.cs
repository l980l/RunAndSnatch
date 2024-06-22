using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/ChronoTwistSkill")]
public class ChronoTwistSkill : SkillEffect
{
    private PlayerMovement playerMovement;

    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            if (playerMovement == null)
            {
                playerMovement = GameManager.Instance.GetPlayer().GetComponent<PlayerMovement>();
            }

            float lastTime = playerMovement.PlayerData.skill.effectLastTime;
            playerMovement.GetComponent<ChronoTwistAbility>().UseChronoTwist(lastTime);
            
            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
