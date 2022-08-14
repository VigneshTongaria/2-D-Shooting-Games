using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallBullet : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    public float returndamage()
    {
        return damage;
    }
}
