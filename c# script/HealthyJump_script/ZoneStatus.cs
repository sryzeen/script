using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneStatus : MonoBehaviour
{
    Rigidbody zone;

    float posMin;
    float posMax;
    public float force = 7f;

    void Awake()
    {
        zone = GetComponent<Rigidbody>();
        posMax = transform.position.y + 0.55f;
        posMin = transform.position.y;
    }

    void FixedUpdate()
    {

        if (transform.position.y < posMin) zone.AddForce(0, force, 0);
        if (transform.position.y > posMax) zone.AddForce(0, -force, 0);
    }
}