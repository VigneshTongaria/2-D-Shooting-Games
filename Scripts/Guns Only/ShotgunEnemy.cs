using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShotgunEnemy : MonoBehaviour
{
    [SerializeField] float delayTime = 1f;
    [SerializeField] float speed = 10f;
    [SerializeField] float bullx;
    [SerializeField] float bully;
    [SerializeField] GameObject bullets;
    [SerializeField] float bullangle;
    [SerializeField] float velbull;
    [SerializeField] float health = 70f;
    [SerializeField] float killedXP = 50f;
    [SerializeField] GameObject healthbar;
    [SerializeField] float healthposy;
    [SerializeField] float healthposx;
    [SerializeField] AudioClip sound;
    Gun player;
    float delay;
    bool reached = true;
    Vector3 previousplayerpos;
    bool posset = false;
    float oghealthx;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        delay = delayTime;
        player = FindObjectOfType<Gun>();
        healthbar = Instantiate(healthbar,transform.position + new Vector3(healthposx,healthposy,0),Quaternion.identity);
        oghealthx = healthbar.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            player.returnspecial(killedXP);
            Destroy(healthbar);
            Destroy(gameObject);
        }
        rotate();
        if (!reached) move();
        fire();
    }
    private void rotate()
    {
        Vector3 diff = player.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }
    private void fire()
    {  
        if(reached)
        {
            reached = false;
            Vector2 buildpos = bullx * transform.right + bully * transform.up + transform.position;
            GameObject bull1 = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z + 2 * bullangle)) as GameObject;
            GameObject bull2 = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z + bullangle)) as GameObject;
            GameObject bull3 = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z)) as GameObject;
            GameObject bull4 = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z - bullangle)) as GameObject;
            GameObject bull5 = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z - 2 * bullangle)) as GameObject;
            bull1.GetComponent<Rigidbody2D>().velocity = (bull1.transform.up) * velbull;
            bull2.GetComponent<Rigidbody2D>().velocity = (bull2.transform.up) * velbull;
            bull3.GetComponent<Rigidbody2D>().velocity = (bull3.transform.up) * velbull;
            bull4.GetComponent<Rigidbody2D>().velocity = (bull4.transform.up) * velbull;
            bull5.GetComponent<Rigidbody2D>().velocity = (bull5.transform.up) * velbull;
            AudioSource.PlayClipAtPoint(sound, transform.position);
            delay = delayTime; 
            posset = false;
        }
    }
    private void move()
    {
        delay -= Time.deltaTime;
        if (delay <= 0)
        {   if (!posset)
            {
                previousplayerpos = player.transform.position;
                posset = true;
            }
        if(transform.position == previousplayerpos) reached = true;
            transform.position = Vector2.MoveTowards(transform.position, previousplayerpos, speed*Time.deltaTime);
            healthbar.transform.position = transform.position + new Vector3(healthposx, healthposy, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && collision.gameObject.layer == 9)
        {
            health -= collision.gameObject.GetComponent<SmallBullet>().returndamage();
            healthbar.transform.localScale = new Vector3(oghealthx * health / 80, healthbar.transform.localScale.y, healthbar.transform.localScale.z);
            Destroy(collision.gameObject);
        }
    }
}
