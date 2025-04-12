using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private PlayerMovement player;
    private Transform coin;

    private Vector3 coinStartPos;
    private Vector3 velocity;
    private float distance = 1;
    private bool isUp;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        coin = GetComponent<Transform>();
    }
    void Update()
    {
        if (coin.position.y >= coinStartPos.y + distance - 0.1f)
            isUp = false;
        else if (coin.position.y <= coinStartPos.y - distance + 0.1f) isUp = true;

        if (isUp)
            coin.position = Vector3.SmoothDamp(coin.position, new Vector3(coin.position.x, coinStartPos.y + distance, coin.position.z), ref velocity, 2f);
        else
            coin.position = Vector3.SmoothDamp(coin.position, new Vector3(coin.position.x, coinStartPos.y - distance, coin.position.z), ref velocity, 2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            player.AddCoins();
    }
}
