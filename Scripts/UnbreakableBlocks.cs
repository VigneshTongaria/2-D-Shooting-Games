using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnbreakableBlocks : MonoBehaviour
{   [SerializeField] GameObject laser;
    [SerializeField] int health = 50;
    [SerializeField] float xp = 2.5f;
    [SerializeField] AudioClip blocksound;
      void Start()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector3(0.4f,0.4f,0.4f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (health <= 0)
        {
            if(laser!=null)Instantiate(laser, new Vector3(transform.position.x, transform.position.y - 1, 0), Quaternion.identity);
            FindObjectOfType<LevelDescription>().returnspecialmeter(xp);
            Destroy(gameObject);
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {   if(collision.gameObject.layer==8)
        AudioSource.PlayClipAtPoint(blocksound, transform.position);
    }

    public void returnhealthwithdamage(int damage)
    {
        health -= damage;
    }
}
