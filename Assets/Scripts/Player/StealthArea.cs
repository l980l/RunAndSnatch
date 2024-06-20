using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;
        if (collision.gameObject.layer == 7)   // Monster Layer
        {
            if (collision.gameObject.GetComponent<Monster>() != null)
            {
                Monster monster = collision.gameObject.GetComponent<Monster>();
                monster.Miss();
            }
        }
    }
}
