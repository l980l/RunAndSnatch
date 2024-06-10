using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(menuName = "ScrittableObject/ItemEftSO/BrightenEft")]
public class BrightenEft : ItemEffect
{
    public override bool ExecuteRole()
    {
        GameManager.Instance.GetPlayer().GetComponent<Player>().gameObject.GetComponentInChildren<Light2D>().pointLightOuterRadius += value1;
        return true;
    }
}
