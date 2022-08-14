using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] int damage = 10;
    private void OnCollisionEnter2D(Collision2D collision)
    {  if (collision.gameObject.layer == 6)
        {
            collision.gameObject.GetComponent<Paddle>().returnhealthwithdamage(damage);
            Destroy(gameObject);
        }
    }
}
