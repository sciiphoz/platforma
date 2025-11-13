using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakpoint : MonoBehaviour
{
    private PlayerMovement player;
    private Transform enemy;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        enemy = transform.parent.GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(enemy.gameObject);

            ApiRequests.AddAchievementAsync(4, PlayerPrefs.GetInt("PlayerID"));

            player.AddScore(2);
            player.Jump(5);
        }
    }
}
