using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine;

public class Target : MonoBehaviour {

    public float health = 100f;
    public float healthMAX = 100f;
    private bool dead = false;

    public void Damaged(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
        
    }

    void Update()
    {
        if (transform.name == "Enemy (Clone)")
        {
            transform.name = "EnemyXD";
        }
        if (transform.position.y < -100f) Destroy(gameObject);
    }

    public float HealthRatio()
    {
        return health / healthMAX;
    }

    public bool EnemyState()
    {
        return dead;
    }

    void Die()
    {
        

        dead = true;
        foreach (Transform child in transform)
        {
            if (child.name != "Leg_R" || child.name != "Leg_L")
            child.gameObject.AddComponent<Rigidbody>();
            child.gameObject.AddComponent<BoxCollider>();
            child.parent = null;
        }
        Destroy(gameObject);
    }
}