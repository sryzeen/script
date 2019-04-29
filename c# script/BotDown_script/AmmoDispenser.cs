using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class AmmoDispenser : MonoBehaviour {

    public GameObject ammo;
    private Quaternion rot;
    private Vector3 pos;
    private float cooldownTime = 50f;
    private float nextAmmo = 0f;

    void Start()
    {
        pos = transform.position;
        rot = transform.rotation;
    }

    void Update()
    {
        if (transform.childCount == 0)
        {
            if (Time.time >= nextAmmo)
            {
                Debug.Log("nextAmmo");
                Instantiate(ammo, pos, rot, this.transform);
            }
        }
        else
        {
            nextAmmo = Time.time + 1f / cooldownTime;
        }
        
    }
}