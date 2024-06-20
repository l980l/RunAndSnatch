using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunArea : MonoBehaviour
{
    private float stunTime = 2f;
    public float StunTime { set {  stunTime = value; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.layer == 7)   // Monster Layer
        {
            if (collision.gameObject.GetComponent<Monster>() != null)
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.OnStunSkill(stunTime);
            }
        }
    }
}
