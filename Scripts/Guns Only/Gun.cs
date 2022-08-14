
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Mathematics;
using System.Collections;
using UnityEngine.SceneManagement;
public class Gun : MonoBehaviour
{
    [SerializeField] float randvel;
    [SerializeField] float minx = 1f;
    [SerializeField] float maxx = 31f;
    [SerializeField] float miny = 1f;
    [SerializeField] float maxy = 17f;
    [SerializeField] public GameObject bullets;
    [SerializeField] GameObject sheild;
    [SerializeField] public float velbull = 50f;
    [SerializeField] float bullrandx, bullrandy;
    [SerializeField] float dashdistance = 2f;
    [SerializeField] float movespeed = 1f;
    [SerializeField] public float bullx, bully, bullangle, shotguntimevalue;
    [SerializeField] AudioClip Glocksound;
    [SerializeField] AudioClip healingsound;
    [SerializeField] AudioClip shotgunsound;
    [SerializeField] Sprite[] GunArray;
    [SerializeField] Camera cam;
    [SerializeField] Vector2[] collidersize;
    [SerializeField] float health;
    [SerializeField] float special;
    [SerializeField] float shotgunXP = -12.5f;
    [SerializeField] float healingvalue = 1f;
    [SerializeField] GameObject healthbar;
    [SerializeField] GameObject specialbar;
    bool sheildon = false;
    bool dashing = false;
    bool healing = false;
    Vector3 targetposition;
    GameObject shield;
    GameObject shld;
    int gunindex = 0;
    float shotguntime;
    // Start is called before the first frame update
    void Start()
    {
        gunindex = 0;
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        shotguntime = shotguntimevalue;
        specialbar.transform.localScale = new Vector3(3 * special / 100, 0.25f, 1f);
    }

    // Update is called once per frame
    void Update()
    {   if (health <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        GunSprite();
        GetComponent<SpriteRenderer>().sprite = GunArray[gunindex];
        Moveplayer();
        Dash();
        rotate();
        Heal();
        Fire(gunindex);
    }

    private void GunSprite()
    {
        if (Input.GetMouseButtonDown(1))
            gunindex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha4))
            gunindex = 1;
        GetComponent<BoxCollider2D>().size = collidersize[gunindex];
    }

    private void Moveplayer()
    {
        if (!dashing)
        {
            Vector2 gunpos = new Vector2(transform.position.x + Input.GetAxis("Horizontal") * Time.deltaTime * randvel,
            transform.position.y + Input.GetAxis("Vertical") * Time.deltaTime * randvel);
            gunpos.x = Mathf.Clamp(gunpos.x, minx, maxx);
            gunpos.y = Mathf.Clamp(gunpos.y, miny, maxy);
            transform.position = gunpos;
        }
    }
    private void rotate()
    {
        //Vector2 playerpos =  cam.WorldToViewportPoint(transform.position );
        //Vector2 mousepos = cam.ScreenToViewportPoint(Input.mousePosition);
        // Vector2 rotator = mousepos - playerpos;
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        //transform.LookAt(Input.mousePosition, transform.up);
        //transform.Rotate(transform.right, 90f);
        //if (mousepos.x  < playerpos.x) transform.rotation = Quaternion.Euler(0, 0,180+Mathf.Rad2Deg * Mathf.Atan(rotator.y / rotator.x));
        //else transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan(rotator.y / rotator.x));
        //float mouserotate =  mousespeed * Input.GetAxis("Mouse Y")*Time.deltaTime;
        //Vector3.RotateTowards(transform.position, Input.mousePosition,10,10);
        //transform.Rotate(Vector3.forward,mouserotate);

    }
    private void dash()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 targetposition = new Vector2(transform.position.x + dashdistance, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetposition, movespeed);
            }
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 targetposition = new Vector2(transform.position.x - dashdistance, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetposition, movespeed);
            }
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 targetposition = new Vector2(transform.position.x, transform.position.y + dashdistance);
                transform.position = Vector2.MoveTowards(transform.position, targetposition, movespeed);
            }
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 targetposition = new Vector2(transform.position.x, transform.position.y - dashdistance);
                transform.position = Vector2.MoveTowards(transform.position, targetposition, movespeed);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Vector2 targetposition = new Vector2(transform.position.x - dashdistance, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetposition, movespeed * Time.deltaTime);
            }
        }

    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            targetposition = new Vector2(transform.position.x + Input.GetAxisRaw("Horizontal") * dashdistance, transform.position.y + Input.GetAxisRaw("Vertical") * dashdistance);
            targetposition.x = Mathf.Clamp(targetposition.x, minx, maxx);
            targetposition.y = Mathf.Clamp(targetposition.y, miny, maxy);
            dashing = true;
        }
        if (dashing)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetposition, movespeed * Time.deltaTime);
            if (transform.position == targetposition) dashing = false;
        }
    }

    private void Fire(int index)
    {
        if (shotguntime >= 0) shotguntime -= Time.deltaTime;
        if (index == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // Vector3 buildpos = new Vector3(transform.position.x + bullrandx*math.cos((math.PI / 180)*(transform.rotation.eulerAngles.z)) - bullrandy*math.sin((math.PI / 180)*(transform.rotation.eulerAngles.z)),
                //                  transform.position.y + bullrandx*math.sin((math.PI/180)*(transform.rotation.eulerAngles.z)) + bullrandy*math.cos((math.PI / 180)*(transform.rotation.eulerAngles.z)), transform.position.z);
                Vector3 buildpos = bullrandx * transform.right + bullrandy * transform.up + transform.position;
                GameObject bull = Instantiate(bullets, buildpos, Quaternion.Euler(0, 0, -90 + transform.rotation.eulerAngles.z)) as GameObject;
                //if (mousepos.x / (23.75f) < playerpos.x)
                // bull.GetComponent<Rigidbody2D>().velocity = new Vector2(-velbull*math.cos((math.PI / 180)*(transform.rotation.eulerAngles.z)), velbull*math.sin((math.PI / 180)*(transform.rotation.eulerAngles.z)));
                //bull.GetComponent<Rigidbody2D>().velocity = new Vector2(velbull * math.cos((math.PI / 180) * (transform.rotation.eulerAngles.z)), velbull * math.sin((math.PI / 180) * (transform.rotation.eulerAngles.z)));
                bull.GetComponent<Rigidbody2D>().velocity = velbull * (bull.transform.up);
                AudioSource.PlayClipAtPoint(Glocksound, transform.position);
            }
        }
        else if (index == 1)
        {
            if (Input.GetMouseButtonDown(0) && shotguntime < 0 && special>=12.5)
            {
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
                shotguntime = shotguntimevalue;
                AudioSource.PlayClipAtPoint(shotgunsound, transform.position);
                returnspecial(shotgunXP);
            }
        }

    }
    private void Heal()
    {
        if(Input.GetKeyDown(KeyCode.Space)) AudioSource.PlayClipAtPoint(healingsound, transform.position);
        else if (Input.GetKey(KeyCode.Space))
        {   healing = true;
            if (health < 100 && special > 0)
            {
                returnspecial(-Time.deltaTime * healingvalue);
                returnhealth(-Time.deltaTime * healingvalue);
            }
        }
        else if(Input.GetKeyUp(KeyCode.Space)||special==0) healing = false;
        
    }
    private void Shield()

    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            while (!sheildon)
            {
                shield = Instantiate(sheild, transform.position, Quaternion.identity) as GameObject;
                sheildon = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            Destroy(shield);
            sheildon = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet" && collision.gameObject.layer == 8 && !healing)
            returnhealth(collision.GetComponent<SmallBullet>().returndamage());
    }
    public void returnspecial(float s)
    {
        special += s;
        if(special<0) special = 0;
        if(special>100) special = 100;
        specialbar.transform.localScale = new Vector3(3 * special / 100, 0.25f, 1f);
    }
    public void returnhealth(float h)
    {
        health -= h;
        if(health<0) health = 0;
        healthbar.transform.localScale = new Vector3(3 * health / 100, 0.25f, 1f);
    }
}
