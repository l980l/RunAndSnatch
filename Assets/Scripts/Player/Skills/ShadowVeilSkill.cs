using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/ShadowVeilSkill")]
public class ShadowVeilSkill : SkillEffect
{
    // 쿨타임은 SkillUI에서 처리하고, 이 함수가 실행되기 때문에, 따로 처리하지 않아도 된다. 
    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            Player player = GameManager.Instance.GetPlayer().GetComponent<Player>();
            float skillRange = player.playerData.skill.skillRange;
            float lastTime = player.playerData.skill.effectLastTime;
            // 은신. 내부적으로 충돌체 OnOff
            Debug.Log(lastTime);
            player.SetStealthForDuration(lastTime, skillRange);

            UpdateLastExecutionTime();
            return true;
        }
        else
        {
            return false;
        }
    }
}
