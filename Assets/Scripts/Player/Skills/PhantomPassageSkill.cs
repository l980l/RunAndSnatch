using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/SkillSO/PhantomPassageSkill")]
public class PhantomPassageSkill : SkillEffect
{
    private float lastTime;
    private PhantomPassageAbility phantomPassageAbility;

    public override bool ExecuteRole()
    {
        if (IsCooltimeReady())
        {
            if(phantomPassageAbility == null)
            {
                lastTime = GameManager.Instance.GetPlayer().GetComponent<Player>().playerData.skill.effectLastTime;

                phantomPassageAbility = GameManager.Instance.GetPlayer().GetComponent<PhantomPassageAbility>();
            }
           
            phantomPassageAbility.UsePhantomPassage(lastTime);
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
