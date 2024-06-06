using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    BlueShell,
    DamageTest
}

public class Item : MonoBehaviour
{
    [SerializeField] public ItemType ItemType;

    private Rigidbody2D Rigidbody2D;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
