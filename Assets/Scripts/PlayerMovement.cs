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
    private float speed = 5f;
    private float jumpForce = 7f;
    
    private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    
    private bool isGrounded;
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
        float move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        background.position = Vector3.SmoothDamp(
            new Vector3(background.position.x, background.position.y, background.position.z),
            new Vector3(rb.position.x * 0.5f + 24, rb.position.y * 0.5f, background.position.z), 
            ref velocity,
            0.5f
            );

        if (move > 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        if (move < 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        animator.SetBool("isRunning", move != 0);
        
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.5f, groundLayer);
        animator.SetBool("isJumping", !isGrounded);

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

    public void Jump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
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
