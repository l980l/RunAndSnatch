using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "ScriptableObject/ItemEftSO/BrightenEft")]
public class BrightenEft : ItemEffect 
{
    private Light2D light2D;
    public override bool ExecuteRole()
    {
        if (light2D == null)
            light2D = GameManager.Instance.GetPlayer().GetComponentInChildren<Light2D>();
        light2D.pointLightOuterRadius += value1;
        SoundManager.Instance.PlaySFX(SFX.BrightenSFX, Camera.main.transform.position);
        
        return true;
    }
}
