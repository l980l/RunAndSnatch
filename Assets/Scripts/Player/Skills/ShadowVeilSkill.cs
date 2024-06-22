using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/ShadowVeilSkill")]
public class ShadowVeilSkill : SkillEffect
{
    private PlayerMovement playerMovement;
    private PlayerStealth playerStealth;

    // 쿨타임은 SkillUI에서 처리하고, 이 함수가 실행되기 때문에, 따로 처리하지 않아도 된다. 
    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            if (playerMovement == null || playerStealth == null)
            {
                playerMovement = GameManager.Instance.GetPlayer().GetComponent<PlayerMovement>();
                playerStealth = GameManager.Instance.GetPlayer().GetComponent<PlayerStealth>();
            }
            float skillRange = playerMovement.PlayerData.skill.skillRange;
            float lastTime = playerMovement.PlayerData.skill.effectLastTime;
            playerStealth.SetStealthForDuration(lastTime, skillRange);

            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
