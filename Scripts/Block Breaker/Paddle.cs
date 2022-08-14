using System.Collections;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
public class Paddle : MonoBehaviour
{
    Ball ball;
    public bool ballrealesed;
    public bool launchingball;
    int tries = 0;
    [SerializeField] int health = 200;
    [SerializeField] float velocitymagnitude = 15;
    [SerializeField] float ypos = 0.25f;
    [SerializeField] GameObject PlayerLaser;
    [SerializeField] float vely = 50f;
    [SerializeField] float laserposx;
    [SerializeField] AudioClip lasersound;
    // Start is called before the first frame update
    void Start()
    {
        ball = FindObjectOfType<Ball>();
        ballrealesed = false;
        launchingball = false;
        gameObject.GetComponent<Transform>().localScale = new Vector3(1f, 0.7f,1f);
    } 

    // Update is called once per frame
    void Update()
    {
        if(health<=0||tries==3) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        float vx = (Input.mousePosition.x / Screen.width) * 16 - transform.position.x;
        float vy = (Input.mousePosition.y / Screen.height) * 9 - transform.position.y;
        if (!launchingball)
            transform.position = new Vector3(Mathf.Clamp((Input.mousePosition.x / Screen.width) * 16, 0, 16), ypos, 0);
        if (!ballrealesed)
        {
            ball.GetComponent<Ball>().returnballpos(transform.position.x);
            if (Input.GetKey(KeyCode.Space))
            {
                Vector2 vel = new Vector2(vx * velocitymagnitude / Mathf.Sqrt(vx * vx + vy * vy), vy * velocitymagnitude / Mathf.Sqrt(vx * vx + vy * vy));
                launchingball = true;
                if (Input.GetMouseButton(1))
                {
                    ball.GetComponent<Ball>().returnballvel(vel);
                    ballrealesed = true;
                    launchingball = false;
                }
            }
        }
        Fire();
    }

    private void Fire()
    {
        if (Input.GetKey(KeyCode.D)&&FindObjectOfType<LevelDescription>().specialmeter>0&&ballrealesed)
        {
            if (Input.GetMouseButtonDown(0)&& SceneManager.GetActiveScene().buildIndex<=5)
            {
                GameObject fire = Instantiate(PlayerLaser, transform.position, Quaternion.identity) as GameObject;
                fire.GetComponent<Rigidbody2D>().velocity = new Vector2(0, vely);
                AudioSource.PlayClipAtPoint(lasersound, transform.position);
            }
            else if(Input.GetMouseButtonDown(0)&& SceneManager.GetActiveScene().buildIndex>5)
            {
                GameObject fire1 = Instantiate(PlayerLaser, transform.position, Quaternion.identity) as GameObject;
                GameObject fire2 = Instantiate(PlayerLaser, transform.position + new Vector3(laserposx,0,0), Quaternion.identity) as GameObject;
                GameObject fire3 = Instantiate(PlayerLaser, transform.position - new Vector3(laserposx,0,0), Quaternion.identity) as GameObject;
                fire1.GetComponent<Rigidbody2D>().velocity = new Vector2(0, vely);
                fire2.GetComponent<Rigidbody2D>().velocity = new Vector2(0, vely);
                fire3.GetComponent<Rigidbody2D>().velocity = new Vector2(0, vely);
            }
        }
    }

    public void returnhealthwithdamage(int damage)
    {
        health = health - damage;
    }
    public int returnhealth()
    {
        return health;
    }
    public void returnstatus()
    {
        ballrealesed = false;
        launchingball = false;
        tries++;
    }



}
