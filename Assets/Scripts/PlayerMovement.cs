using Assets.Scripts;
using System.Collections;
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
        PlayerPrefs.SetFloat("Music", 1);
        PlayerPrefs.SetFloat("Sound", 1);
    }
    void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<Transform>();

        starCounter = GameObject.Find("StarCounter").GetComponent<Text>();
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
            ApiRequests.AddAchievementAsync(3, PlayerPrefs.GetInt("PlayerID"));
            StartCoroutine(Lose());
        }

        audioSource.volume = PlayerPrefs.GetFloat("Sound");

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

            if (Input.GetKeyDown(KeyCode.Escape) && settingsOpened == false)
            {
                StartCoroutine(Pause());
                settingsOpened = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && settingsOpened == true)
            {
                isPaused = false;
                settingsAnimator.Play("CloseSettings");
                settingsOpened = false;
            }
        }
    }
    public IEnumerator Win()
    {
        if (score > 150)
        {
            ApiRequests.AddAchievementAsync(5, PlayerPrefs.GetInt("PlayerID"));
        }

        if (health == 1)
        {
            ApiRequests.AddAchievementAsync(1, PlayerPrefs.GetInt("PlayerID"));
        }

        switch (SceneManager.GetActiveScene().name)
        {
            case ("Level1"):
                PlayerPrefs.SetInt("level1Score", score);
                ApiRequests.AddScore(PlayerPrefs.GetInt("PlayerID"), score, 1);
                break;
            case ("Level2"):
                PlayerPrefs.SetInt("level2Score", score);
                ApiRequests.AddScore(PlayerPrefs.GetInt("PlayerID"), score, 2);
                break;
            case ("Level3"):
                PlayerPrefs.SetInt("level3Score", score);
                ApiRequests.AddScore(PlayerPrefs.GetInt("PlayerID"), score, 3);
                break;
            default:
                Debug.Log("Error occured");
                break;
        }

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
    public IEnumerator Pause()
    {
        settingsAnimator.Play("OpenSettings");
        yield return new WaitForSeconds(0.5f);
        isPaused = true;
    }
    public void TakeDamage(int damageValue)
    {
        if (isDamaged == false)
        {
            health -= damageValue;
            AddScore(-1);

            StartCoroutine(IFramesCounter());
            StartCoroutine(IFramesAnimation());
            UpdateUI();
        }
    }

    public void PlayCoinSound(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void PlayStarSound(AudioClip audio)
    {
        audioSource.PlayOneShot(audio);
    }

    public void TakeDamage(int damageValue, bool ignore)
    {
        health -= damageValue;
        AddScore(-1);

        StartCoroutine(IFramesCounter());
        StartCoroutine(IFramesAnimation());
        UpdateUI();
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
            rb.velocity = new Vector2(7.5f, rb.velocity.y);
        }
        else rb.velocity = new Vector2(-7.5f, rb.velocity.y);

        StartCoroutine(DashCooldown());
    }
    public IEnumerator AnchorY()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.75f);
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
            if (child.CompareTag("Heart"))
                Destroy(child.gameObject);
        }

        for (int i = 0; i < health; i++)
        {
            Instantiate(heart, new Vector3(playerUI.position.x - 8 + (1.05f * i), playerUI.position.y + 4.5f, 0), Quaternion.identity, playerUI);
        }

        scoreCounter.text = "score: " + score.ToString();
        starCounter.text = starCount.ToString();
    }
    public void AddScore(int mult)
    {
        score += 5 * mult;
        UpdateUI();
    }

    public void AddStars()
    {
        starCount++;
        AddScore(2);
        if (health != 3) health++;
        UpdateUI();
    }
}
