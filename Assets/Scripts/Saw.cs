using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private AudioClip sawSound;
    private AudioSource audioSource;

    private PlayerMovement player;

    private Transform saw;
    private float rotationSpeed = 180f;
    void Start()
    {
        saw = GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = sawSound;
        audioSource.Play();
    }

    void Update()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            audioSource.volume = 0.15f;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Debug.Log(1);
            player.TakeDamage(1);
        }
    }
}
