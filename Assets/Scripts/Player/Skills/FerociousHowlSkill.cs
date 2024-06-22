using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/FerociousHowlSkill")]
public class FerociousHowlSkill : SkillEffect
{
    private PlayerMovement playerMovement;
    private PlayerStun playerStun;

    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            if (playerMovement == null)
            {
                playerMovement = GameManager.Instance.GetPlayer().GetComponent<PlayerMovement>();
                playerStun = GameManager.Instance.GetPlayer().GetComponent<PlayerStun>();
            }
            float skillRange = playerMovement.PlayerData.skill.skillRange;
            float lastTime = playerMovement.PlayerData.skill.effectLastTime;
            playerStun.SetStunAreaForDuration(lastTime, skillRange);

            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
