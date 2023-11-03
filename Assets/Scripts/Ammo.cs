using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    private new Rigidbody2D rigidbody;
    public float speed = 500f;
    public float maxLifetime = 10f;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Project(Vector2 direction)
    {
        rigidbody.AddForce(direction * speed);
        Destroy(this.gameObject, this.maxLifetime);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
