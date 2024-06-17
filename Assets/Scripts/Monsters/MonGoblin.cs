using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGoblin : Monster
{
    [SerializeField] private BoxCollider2D atkBox;

    protected override void Awake()
    {
        base.Awake();
        atkBox.enabled = false;
    }

    protected override void Attack()
    {
        if (atkBox != null)
        {
            FlipCollider(GetComponent<SpriteRenderer>().flipX);
            atkBox.enabled = true;
            StartCoroutine(DisableAtkBox(0.1f));
        }
    }

    private IEnumerator DisableAtkBox(float _delay)
    {
        yield return new WaitForSeconds(_delay);
        if (atkBox != null)
        {
            atkBox.enabled = false;
        }
    }

    protected void FlipCollider(bool _FlipX)
    {
        if(atkBox != null)
        {
            if(_FlipX)
            {
                if (atkBox.offset.x > 0)
                {
                    Vector3 colliderPos = atkBox.offset;
                    colliderPos.x = -colliderPos.x;
                    atkBox.offset = colliderPos;
                }
            }
            else
            {
                if (atkBox.offset.x < 0)
                {
                    Vector3 colliderPos = atkBox.offset;
                    colliderPos.x = -colliderPos.x;
                    atkBox.offset = colliderPos;
                }
            }
        }
    }
}
