using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public new Rigidbody2D rigidbody;
    public Ammo ammoShot;

    public float thustSpeed = 1.0f;
    public bool thrusting { get; private set; }

    public float turnSpeed = 0.1f;
    private float turnDirection { get; set; } = 0f;

    public float respawnDelay = 3f;
    public float respawnInvulnerability = 3f;

    public bool screenWrapping = true;
    private Bounds screenBounds;

    public int playerlife { get; set; }

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerlife = 3;
    }

    

    private void Start()
    {
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag("Boundary");

        for (int i = 0; i < boundaries.Length; i++)
        {
            boundaries[i].SetActive(!screenWrapping);
        }

        screenBounds = new Bounds();
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(Vector3.zero));
        screenBounds.Encapsulate(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f)));
    }

    private void OnEnable()
    {
        TurnOffCollisions();
        Invoke(nameof(TurnOnCollisions), respawnInvulnerability);
    }

    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1f;
        }
        else
        {
            turnDirection = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        if (thrusting)
        {
            rigidbody.AddForce(this.transform.up * this.thustSpeed);
        }

        if (turnDirection != 0f)
        {
            rigidbody.AddTorque(this.turnDirection * this.turnSpeed);
        }

        if (screenWrapping)
        {
            ScreenWrap();
        }
    }

    private void ScreenWrap()
    {
        if (rigidbody.position.x > screenBounds.max.x + 0.5f)
        {
            rigidbody.position = new Vector2(screenBounds.min.x - 0.5f, rigidbody.position.y);
        }
        else if (rigidbody.position.x < screenBounds.min.x - 0.5f)
        {
            rigidbody.position = new Vector2(screenBounds.max.x + 0.5f, rigidbody.position.y);
        }
        else if (rigidbody.position.y > screenBounds.max.y + 0.5f)
        {
            rigidbody.position = new Vector2(rigidbody.position.x, screenBounds.min.y - 0.5f);
        }
        else if (rigidbody.position.y < screenBounds.min.y - 0.5f)
        {
            rigidbody.position = new Vector2(rigidbody.position.x, screenBounds.max.y + 0.5f);
        }
    }

    private void Shoot()
    {
        Ammo ammo = Instantiate(this.ammoShot, this.transform.position, this.transform.rotation);
        ammo.Project(this.transform.up);
    }

    private void TurnOffCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Colisions");
    }

    private void TurnOnCollisions()
    {
        gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            playerlife--;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = 0f;

            GameManager.Instance.PlayerDied(this);
        }
    }
}
