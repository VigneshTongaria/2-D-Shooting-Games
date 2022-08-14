using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OFFscreencolldier : MonoBehaviour
{
    Paddle paddle;
    private void OnCollisionEnter2D(Collision2D collision)
    {   if (collision.gameObject.layer == 8 && collision.gameObject.tag == "Player")
        {
            paddle = FindObjectOfType<Paddle>();
            paddle.returnstatus();
        }
        else 
            Destroy(collision.gameObject);
    }
}
