using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/BellEft")]
public class BellEft : ItemEffect 
{
    private PlayerStun playerStun;
    public override bool ExecuteRole()
    {
        if (playerStun == null)
            playerStun = GameManager.Instance.GetPlayer().GetComponent<PlayerStun>();
        playerStun.SetStunAreaForDuration(value1, 13f);
        CamShake.Instance.ShakeCamera(3f, 1f);

        SoundManager.Instance.PlaySFX(SFX.BellSFX, Camera.main.transform.position);

        return true;
    }
}
