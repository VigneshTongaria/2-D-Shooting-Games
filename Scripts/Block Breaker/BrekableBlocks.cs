using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrekableBlocks : MonoBehaviour
{
    [SerializeField] Sprite CrackedBlock;
    [SerializeField] GameObject particle;
    [SerializeField] GameObject laser;
    [SerializeField] int hit = 3;
    [SerializeField] int xp;
    [SerializeField] AudioClip blocksound;
    int hits = 0;
    LevelDescription level;
    void Start()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.22f,0.3f);
        level = FindObjectOfType<LevelDescription>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioSource.PlayClipAtPoint(blocksound, transform.position);
        hits++;
        if (hits == hit)
        {
           if(laser!=null) Instantiate(laser,transform.position,Quaternion.identity);
            level.returnspecialmeter(xp);
            Destroy(gameObject);
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = CrackedBlock;
            GameObject part = Instantiate(particle, collision.transform.position, Quaternion.Euler(90,0,0));
            Destroy(part, 1f);
        }
    }
}
