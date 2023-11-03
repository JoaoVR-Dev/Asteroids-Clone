using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Asteroids : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }
    public new BoxCollider2D collider { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public Sprite[] sprites;

    public float size = 10f;
    public float minSize = 5f;
    public float maxSize = 20f;

    public float speed = 50.0f;
    public float maxLifeTime = 30.0f;
    public int asteroidLife { get; private set; }
    public int spriteint { get; private set; }
    public float medium { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        medium = maxSize - minSize;
        if (this.size >= medium * 0.75f + minSize)
        {
            spriteint = 3;
            asteroidLife = 3;
        }
        else if (size <= medium * 0.25f + minSize)
        {
            spriteint = 0;
            asteroidLife = 1;
        }
        else
        {
            spriteint = 1;
            asteroidLife = 2;
        }
        spriteRenderer.sprite = sprites[spriteint];
        collider.size = spriteRenderer.sprite.bounds.size;
        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * size;
        rigidbody.mass = size*10;
        Destroy(gameObject, maxLifeTime);
    }

    public void SetTrajectory(Vector2 direction)
    {
        rigidbody.AddForce(direction * this.speed);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ammo"))
        {
            asteroidLife--;
            
            if (asteroidLife <= 0)
            {
                if (this.size >= this.medium * 0.25f + minSize)
                {
                    CreateSplit();
                    CreateSplit();
                }
                GameManager.Instance.AsteroidDestroyed(this);
                Destroy(this.gameObject);
            }
            else
            {
                spriteint++;
                spriteRenderer.sprite = sprites[spriteint];
                GameManager.Instance.AsteroidDestroyed(this);
            }
        }

    }

    public Asteroids CreateSplit()
    {
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;
        Asteroids half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;
        half.SetTrajectory(Random.insideUnitCircle.normalized);
        return half;
    }
}
