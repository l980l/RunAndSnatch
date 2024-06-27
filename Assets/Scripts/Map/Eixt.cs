using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eixt : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == GameManager.Instance.GetPlayer())
            ExitUI.instance.OpenExitUI();
    }
}
