using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] Wave wave;
    // Update is called once per frame
    void Update()
    {
        if (!FindObjectOfType<GlockEnemy>() && !FindObjectOfType<ShotgunEnemy>() && wave.returnwavestatus())
        {
            if(SceneManager.GetActiveScene().buildIndex!=3) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
            else SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
        if(Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
