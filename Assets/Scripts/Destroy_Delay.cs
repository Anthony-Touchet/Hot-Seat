using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Delay : MonoBehaviour
{
    public float timeAlive;
    private float initalTime;

    // Start is called before the first frame update
    void Start()
    {
        initalTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= initalTime + timeAlive)
        {
            Destroy(this.gameObject);
        }
    }
}
