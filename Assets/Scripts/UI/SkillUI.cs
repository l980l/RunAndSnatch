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

    private PlayerMovement player;
    private SkillEffect skillEffect;
    private float coolTime;
    private bool canUseSkill;

    public void SetPlayer(PlayerMovement _player)
    {
        player = _player;
        skillEffect = player.PlayerData.skill;
        if (skillEffect != null)
        {
            // 스킬 아이콘 세팅
            skillButton.image.sprite = player.PlayerData.skill.UIImage;
            coolTime = skillEffect.coolTime;
            skillEffect.lastExecutionTime = 0f;
        }
        else
            gameObject.SetActive(false);
    }

    private void Update()
    {
        if (skillEffect)
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
        if(player != null && NetworkManager.Instance.IsNetworkConnected)
        {
            float flownTime = 0f;
            // 네트워크가 끊기면 스킬을 못쓰니, 이렇게만 비교하면 스킬을 쓰고 네트워크가 끊겼는지 확인할 수 있다.
            if (NetworkManager.Instance.LastDisconnectTime < skillEffect.lastExecutionTime)
                flownTime = Time.realtimeSinceStartup - skillEffect.lastExecutionTime;
            // 스킬 사용 후 네트워크가 끊겼다가 다시 풀린 경우. 
            else
                flownTime = Time.realtimeSinceStartup - NetworkManager.Instance.DisconnectTime - skillEffect.lastExecutionTime;

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
        if(skillEffect && NetworkManager.Instance.IsNetworkConnected)
        {
            canUseSkill = false; // 사용 불가능으로 변경
            skillEffect.ExecuteRole();
        }
    }
}
