using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private Transform heart;

    private Transform playerUI;

    private Transform background;
    private Vector3 velocity;

    private int health = 3;
    private float speed = 4f;
    private float jumpForce = 7f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    
    private bool isGrounded;

    internal bool dashReady = true;
    void Start()
    {
        playerUI = GameObject.Find("PlayerUI").GetComponent<Transform>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        background = GameObject.Find("BackgroundUI").GetComponent<Transform>();
        UpdateUI();
    }

    void Update()
    {
        Debug.Log(dashReady);

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
        
        if (health == 0)
        {
            Time.timeScale = 0;
        }
    }

    public void TakeDamage(int damageValue)
    {
        health -= damageValue;

        UpdateUI();
    }

    public void Dash()
    {
        if (transform.localScale.x > 0)
        {
            rb.velocity = new Vector2(10f, rb.velocity.y);
        }
        else rb.velocity = new Vector2(-10f, rb.velocity.y);

        StartCoroutine(DashCooldown());
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
            Instantiate(heart, new Vector3(-8 + 1 * i, 4, 0), Quaternion.identity, playerUI);
        }
    }

    public void AddCoins()
    {

    }

    public void AddStars()
    {

    }
}
