using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Unity.Mathematics;

public class GlockEnemy : MonoBehaviour
{   
     Gun player;
    [SerializeField] float movespeed = 0.1f;
    [SerializeField] float minx = 0f;
    [SerializeField] float maxx = 32f;
    [SerializeField] float miny = 0f;
    [SerializeField] float maxy = 17f;
    [SerializeField] float range = 2f;
    [SerializeField] float velbull = 50f;
    [SerializeField] float bullrandx;
    [SerializeField] float bullrandy;
    [SerializeField] GameObject bullets;
    [SerializeField] AudioClip Glocksound;
    [SerializeField] float minrate=0.4f;
    [SerializeField] float maxrate=1f;
    [SerializeField] float health = 200f;
    [SerializeField] float KilledXP = 30f;
    [SerializeField] GameObject healthbar;
    [SerializeField] float healthypos;
    [SerializeField] float healthposx;
    bool reached;
    float oghealthscalex;
    Vector2 newpos;
    float RandRate;
    // Start is called before the first frame update
    void Start()
    {
        reached = true;
        RandRate = UnityEngine.Random.Range(minrate, maxrate);
        player = FindObjectOfType<Gun>();
        healthbar = Instantiate(healthbar,new Vector3(transform.position.x, transform.position.y + healthypos, transform.position.z),Quaternion.identity);
        oghealthscalex = healthbar.GetComponent<Transform>().localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        rotate();
        RandRate -= Time.deltaTime;
        healthbar.transform.position = transform.position + new Vector3(healthposx, healthypos, 0);
        if (RandRate <= 0) Shoot();
        if (health <= 0)
        {
            player.returnspecial(KilledXP);
            Destroy(healthbar);
            Destroy(gameObject);
        }
     
    }

    private void Move()
    {
        if (reached == true)
        {
            newpos = new Vector2(transform.position.x + UnityEngine.Random.Range((-1) * range, range), transform.position.y + UnityEngine.Random.Range((-1) * range, range));
            newpos.x = Mathf.Clamp(newpos.x, minx, maxx);
            newpos.y = Mathf.Clamp(newpos.y, miny, maxy);
            reached = false;
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, newpos, movespeed * Time.deltaTime);
            if (transform.position.x == newpos.x && transform.position.y == newpos.y) { reached = true; }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && collision.gameObject.layer == 9)
        {
            float damage = collision.gameObject.GetComponent<SmallBullet>().returndamage();
            takedamage(damage);
            Destroy(collision.gameObject);      
        }
    }

    private void  Shoot()
    {

        
        RandRate = UnityEngine.Random.Range(minrate, maxrate);
        var vx = velbull;
        if (player.transform.position.x < transform.position.x)
            vx = -vx;
       // Vector3 buildpos = new Vector3(transform.position.x + bullrandx * math.cos((math.PI / 180) * (transform.rotation.eulerAngles.z)) - bullrandy * math.sin((math.PI / 180) * (transform.rotation.eulerAngles.z)),
         //                    transform.position.y + bullrandx * math.sin((math.PI / 180) * (transform.rotation.eulerAngles.z)) + bullrandy * math.cos((math.PI / 180) * (transform.rotation.eulerAngles.z)), transform.position.z);
        Vector3 buildpos = bullrandx * transform.right + bullrandy * transform.up + transform.position;
        GameObject bull = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z)) as GameObject;
        bull.GetComponent<Rigidbody2D>().velocity = velbull * (bull.transform.up);
        AudioSource.PlayClipAtPoint(Glocksound, transform.position);

    }      
    private void takedamage(float damage)
    {
        health -= damage;
        healthbar.GetComponent<Transform>().localScale = new Vector3(oghealthscalex*health / 100, healthbar.GetComponent<Transform>().localScale.y, healthbar.GetComponent<Transform>().localScale.z);
    }
    private void rotate()
    {
        if (player.transform.position.x < transform.position.x)
            transform.rotation = Quaternion.Euler(0, 0, 180 +(float)(Mathf.Rad2Deg * Math.Atan((transform.position.y - player.transform.position.y) / (transform.position.x - player.transform.position.x))));
        else
            transform.rotation = Quaternion.Euler(0, 0, (float)(Mathf.Rad2Deg * Math.Atan((transform.position.y - player.transform.position.y) / (transform.position.x - player.transform.position.x))));
    }    

    public float returnhealth()
    {
        return health;
    }

}
