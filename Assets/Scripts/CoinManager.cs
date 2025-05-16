using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private PlayerMovement player;

    [SerializeField] private AudioClip coinPickUpSound1;
    [SerializeField] private AudioClip coinPickUpSound2;
    [SerializeField] private AudioClip coinPickUpSound3;

    private int pickedSound;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        pickedSound = Random.Range(1, 4);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            switch (pickedSound)
            {
                case 1:
                    player.PlayCoinSound(coinPickUpSound1);
                    break;
                case 2:
                    player.PlayCoinSound(coinPickUpSound2);
                    break;
                case 3:
                    player.PlayCoinSound(coinPickUpSound3);
                    break;
                default:
                    player.PlayCoinSound(coinPickUpSound1);
                    break;
            }

            player.AddScore(1);
            Destroy(transform.gameObject);
        }
    }
}
