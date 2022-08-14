using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] GameObject[] wave;
    [SerializeField] Vector3[] wavebuildpos;
    [SerializeField] GameObject previouswave;
    bool wavedestroyed = false;
    int destroycount = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (previouswave == null)
        {
            for (int i = 0; i < wave.Length; i++)
            {
                Instantiate(wave[i], wavebuildpos[i], Quaternion.identity);
                wavedestroyed = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
        if (!FindObjectOfType<GlockEnemy>()&&!FindObjectOfType<ShotgunEnemy>()&&previouswave.GetComponent<Wave>().returnwavestatus() == true)
        {   Destroy(previouswave);
            for (int i = 0; i < wave.Length; i++)
            {
                Instantiate(wave[i], wavebuildpos[i], Quaternion.identity);
                wavedestroyed = true;
            }
        }
    }

    public bool returnwavestatus()
    {
        return wavedestroyed;
    }

}
