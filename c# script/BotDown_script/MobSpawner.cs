using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {

    public Transform mob;
    public float spawnRate = 1f;
    private float spawn = 0f;
    private Quaternion rot;
    private Vector3 pos;
    private int counter = 0;
    public int limiter = 5;

	// Use this for initialization
	void Start () {
        pos = transform.position;
        rot = transform.rotation;
	}

    // Update is called once per frame
    void Update()
    {
        if (counter < limiter)
        {
            if (Time.time >= spawn)
            {
                spawn = Time.time + 1f / spawnRate;
                Instantiate(mob, pos, rot, this.transform);
                counter ++;
                //Debug.Log(counter);
            }
            if (counter > transform.childCount) counter = transform.childCount - 1;
            //Debug.Log(counter);
        }
    }
}
