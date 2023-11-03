using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public int score { get; private set; }

    [SerializeField] private Player player;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (player.playerlife <= 0 && Input.GetKeyDown(KeyCode.Return))
        {
            NewGame();
        }
    }

    private void NewGame()
    {
        Asteroids[] asteroids = FindObjectsOfType<Asteroids>();

        for (int i = 0; i < asteroids.Length; i++)
        {
            Destroy(asteroids[i].gameObject);
        }

        gameOverUI.SetActive(false);

        SetScore(0);
        SetLives(3);
        Respawn();

    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }

    private void SetLives(int lives)
    {
        player.playerlife = lives;
        livesText.text = lives.ToString();
    }

    private void Respawn()
    {
        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
    }

    public void PlayerDied(Player player)
    {
        player.gameObject.SetActive(false);

        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        SetLives(player.playerlife);
        if (this.player.playerlife <= 0)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Invoke(nameof(Respawn), player.respawnDelay);
        }
    }

    public void AsteroidDestroyed(Asteroids asteroids)
    {
        if (asteroids.asteroidLife <= 0)
        {
            this.explosion.transform.position = asteroids.transform.position;
            this.explosion.Play();
            if (asteroids.size < asteroids.medium * 0.25f + asteroids.minSize)
            {
                SetScore(score + 100);
            }
            else if (asteroids.size > asteroids.medium * 0.75f + asteroids.minSize)
            {
                SetScore(score + 25);
            }
            else
            {
                SetScore(score + 50);
            }
        }
        else
        {
            SetScore(score + 10);
        }
    }
}
