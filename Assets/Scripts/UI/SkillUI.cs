using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image maskImage;
    [SerializeField] private Button skillButton;
    [SerializeField] private Text coolTimeText;

    private Player player;
    private SkillEffect skillEffect;
    private float coolTime;
    private bool canUseSkill;

    public void SetPlayer(Player _player)
    {
        player = _player;
        skillEffect = player.playerData.skill;
        if (skillEffect != null)
        {
            // 스킬 아이콘 세팅
            skillButton.image.sprite = player.playerData.skill.UIImage;
            coolTime = skillEffect.coolTime;
        }
        else
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if(skillEffect)
        {
            // 쿨타임이면 UpdateCoolTime
            if (!canUseSkill)
            {
                UpdateCoolTime();
            }

            // 사용 가능한 상태라면 input 받기
            else
            {
                // 스페이스 바를 누르면 호출.
                if (Input.GetButtonDown("Jump"))
                {
                    // 버튼을 누르는 것으로도 호출 가능.
                    Click();
                }
            }
        }
    }

    private void UpdateCoolTime()
    {
        if(player != null)
        {
            float flownTime = Time.time - skillEffect.lastExecutionTime;

            // MaskImage FillAmount 세팅
            float percent = flownTime / coolTime;
            // 1부터 0으로 가야됨.
            percent = 1 - percent;
            maskImage.fillAmount = percent;

            // 남은 쿨타임 세팅
            float leftTime = coolTime - flownTime;
            // 0 이상인 경우에만 숫자 출력.
            if (leftTime > 0)
                coolTimeText.text = leftTime.ToString("F1");
            else
            {
                coolTimeText.text = "";
                canUseSkill = true; // 사용 가능으로 변경
            }
        }
    }

    public void Click()
    {
        if(skillEffect)
        {
            canUseSkill = false; // 사용 불가능으로 변경
            skillEffect.ExecuteRole();
        }
    }
}
