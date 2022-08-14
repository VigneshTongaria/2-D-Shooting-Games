using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public class EnemyGlock : MonoBehaviour
{
    [SerializeField] public int health = 200;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        health -= 20;
    }
}
