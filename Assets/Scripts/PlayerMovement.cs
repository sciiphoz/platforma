using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int levelStarsCount;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private Transform heart;

    private Text starCounter;
    private Text scoreCounter;

    private Transform playerUI;

    private Transform background;
    private Vector3 velocity;

    private int health = 3;
    private float speed = 4f;
    private float jumpForce = 7f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;

    private Animator winAnimator;
    private Animator loseAnimator;

    private Animator settingsAnimator;
    private bool settingsOpened = false;
    
    private bool isGrounded;
    private bool isDamaged = false;
    private int starCount = 0;
    private int score = 0;

    private bool dashReady = true;

    private bool isPlaying = true;
    private bool isPaused = false;
    private void Awake()
    {
        PlayerPrefs.SetInt("Music", 1);
        PlayerPrefs.SetInt("Sound", 1);
    }
    void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<Transform>();

        //starCounter = GameObject.Find("StarCounter").GetComponent<Text>();
        scoreCounter = GameObject.Find("ScoreCounter").GetComponent<Text>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        winAnimator = GameObject.Find("Win").GetComponent<Animator>();
        loseAnimator = GameObject.Find("Lose").GetComponent<Animator>();

        settingsAnimator = GameObject.Find("Settings").GetComponent<Animator>();

        background = GameObject.Find("BackgroundUI").GetComponent<Transform>();
        UpdateUI();
    }

    void Update()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (starCount == levelStarsCount)
        {
            isPlaying = false;
            StartCoroutine(Win());
        }

        if (health <= 0)
        {
            isPlaying = false;
            StartCoroutine(Lose());
        }

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioSource.volume = 0.15f;
        }
        else
        {
            audioSource.volume = 0f;
        }

        if (isPlaying)
        {
            float move = Input.GetAxisRaw("Horizontal");

            background.position = Vector3.SmoothDamp(
                new Vector3(background.position.x, background.position.y, background.position.z),
                new Vector3(rb.position.x * 0.5f + 24, rb.position.y * 0.5f, background.position.z),
                ref velocity,
                0.5f
                );

            if (move > 0)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                rb.velocity = new Vector2(move * speed, rb.velocity.y);
            }

            if (move < 0)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                rb.velocity = new Vector2(move * speed, rb.velocity.y);
            }

            animator.SetBool("isRunning", move != 0);

            isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
            animator.SetBool("isJumping", !isGrounded);

            if (Input.GetKeyDown(KeyCode.LeftShift) && dashReady)
            {
                Dash();
            }

            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGrounded)
            {
                Jump(jumpForce);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isPaused = true;
                settingsAnimator.Play("OpenSettings");
                settingsOpened = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape) && settingsOpened)
            {
                isPaused = false;
                settingsAnimator.Play("CloseSettings");
                settingsOpened = false;
            }
        }
    }
    public IEnumerator Win()
    {
        winAnimator.Play("WinAnimation");
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LevelMenu");
    }

    public IEnumerator Lose()
    {
        loseAnimator.Play("LoseAnimation");
        yield return new WaitForSeconds(0.5f);
        Time.timeScale = 0;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("LevelMenu");
    }

    public void TakeDamage(int damageValue)
    {
        if (isDamaged == false)
        {
            health -= damageValue;

            StartCoroutine(IFramesCounter());
            StartCoroutine(IFramesAnimation());
            UpdateUI();
        }
    }

    public IEnumerator IFramesCounter()
    {
        isDamaged = true;
        yield return new WaitForSeconds(0.8f);
        isDamaged = false;
    }
    public IEnumerator IFramesAnimation()
    {
        var material = transform.gameObject.GetComponent<Renderer>().material;
        Color oldColor = material.color;

        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0);
        yield return new WaitForSeconds(0.1f);
        material.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1);
        yield return new WaitForSeconds(0.1f);
    }

    public void Dash()
    {
        StartCoroutine(AnchorY());

        if (transform.localScale.x > 0)
        {
            rb.velocity = new Vector2(10f, rb.velocity.y);
        }
        else rb.velocity = new Vector2(-10f, rb.velocity.y);

        StartCoroutine(DashCooldown());
    }
    public IEnumerator AnchorY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.2f);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    } 
    public IEnumerator DashCooldown()
    {
        dashReady = false;
        yield return new WaitForSeconds(3);
        dashReady = true;
    }

    public void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
        audioSource.PlayOneShot(jumpSound, 0.5f);
    }

    public void Jump(float forceX, float force)
    {
        if (transform.localScale.x > 0)
        {
            rb.velocity = new Vector2(forceX, force);
        }
        else rb.velocity = new Vector2(-forceX, force);

        audioSource.PlayOneShot(jumpSound, 0.5f);
    }

    public void UpdateUI()
    {
        foreach (Transform child in playerUI.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < health; i++)
        {
            Instantiate(heart, new Vector3(playerUI.position.x - 8 + (1.05f * i), playerUI.position.y + 4, 0), Quaternion.identity, playerUI);
        }

        scoreCounter.text = "score: " + score.ToString();
    }
    public void AddScore()
    {
        score += 5;
        UpdateUI();
    }

    public void AddStars()
    {
        starCount++;
        UpdateUI();
    }
}
