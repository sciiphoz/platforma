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
        enemy = GetComponentInParent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(enemy.gameObject);

            player.Jump(5);
        }
    }
}
