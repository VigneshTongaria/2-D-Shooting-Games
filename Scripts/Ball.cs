using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float bally = 0.2f;
    [SerializeField] float velocity;
    [SerializeField] float slowtimesvel = 0.3f;
    [SerializeField] int spvalue = 5;
    [SerializeField] int paddleballxp = 10;
    [SerializeField] AudioClip balltopaddlesound;
    [SerializeField] AudioClip slowtimesound;
    LevelDescription level;
    bool timeslowed = false;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Transform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
        level = FindObjectOfType<LevelDescription>();
    }
        

    // Update is called once per frame
    void Update()
    {

        SlowTime();
        if (GetComponent<Rigidbody2D>().velocity.magnitude >= velocity) GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, velocity);
    }

    private void SlowTime()
    {
        if (Input.GetKeyDown(KeyCode.D) && level.specialmeter > 0)
        {
            GetComponent<Rigidbody2D>().velocity = slowtimesvel * GetComponent<Rigidbody2D>().velocity;
            timeslowed = true;
            AudioSource.PlayClipAtPoint(slowtimesound, transform.position);
        }
        else if (Input.GetKey(KeyCode.D) && level.specialmeter > 0 && timeslowed)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, velocity);
            level.returnspecialmeter(-spvalue * Time.deltaTime);
        }
        else if ((Input.GetKeyUp(KeyCode.D)&& timeslowed)||level.specialmeter==0)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity / slowtimesvel;
            timeslowed = false;
        }
    }

    public void returnballvel(Vector2 vel)
    {
        Vector2 vec = Vector2.ClampMagnitude(vel, velocity);
        GetComponent<Rigidbody2D>().velocity = new Vector2(vec.x, vec.y);
    }

    public void returnballpos(float x)
    {
        transform.position = new Vector2(x, bally);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6&&FindObjectOfType<Paddle>().ballrealesed)
        {
            level.returnspecialmeter(paddleballxp);
            AudioSource.PlayClipAtPoint(balltopaddlesound, transform.position);
        }
    }
}
