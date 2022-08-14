using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    [SerializeField] int damage = 10;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 10)
        {
            collision.gameObject.GetComponent<UnbreakableBlocks>().returnhealthwithdamage(damage);
            Destroy(gameObject);
        }
    }
}
