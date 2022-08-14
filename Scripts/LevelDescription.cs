using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDescription : MonoBehaviour
{
    [SerializeField] GameObject health;
    [SerializeField] GameObject special;
    Paddle paddle;
    [SerializeField] public float specialmeter = 100f;
   void Start()
    {
       paddle = FindObjectOfType<Paddle>();
    }

    void Update()
    {
        if (!FindObjectOfType<BrekableBlocks>()&&!FindObjectOfType<UnbreakableBlocks>())
        {  if(SceneManager.GetActiveScene().buildIndex<10)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1,LoadSceneMode.Single);
           else SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        health.GetComponent<Transform>().localScale = new Vector3(0.2f * paddle.returnhealth() / 200f, health.GetComponent<Transform>().localScale.y, health.GetComponent<Transform>().localScale.z);
        special.GetComponent<Transform>().localScale = new Vector3(0.2f * specialmeter / 100f, special.GetComponent<Transform>().localScale.y, special.GetComponent<Transform>().localScale.z);
    }
    public void returnspecialmeter(float value)
    {
        specialmeter += value;
        if (specialmeter >= 100) specialmeter = 100;
        if(specialmeter < 0) specialmeter = 0; 
    }
    
}
