using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float distance;
    private Transform enemy;
    private Vector3 startPos;
    private bool isRight = true;

    private PlayerMovement player;
    void Start()
    {
        enemy = GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        startPos = enemy.position;
    }

    void Update()
    {
        if (enemy.position.x >= startPos.x + distance)
            isRight = false;
        else if (enemy.position.x <= startPos.x - distance)
            isRight = true;

        if (isRight)
        {
            enemy.position = Vector3.MoveTowards(enemy.position, new Vector3(startPos.x + distance, startPos.y, startPos.z), 0.005f);
            enemy.localScale = new Vector3(-Mathf.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
        }
        else
        {
            enemy.position = Vector3.MoveTowards(enemy.position, new Vector3(startPos.x - distance, startPos.y, startPos.z), 0.005f);
            enemy.localScale = new Vector3(Mathf.Abs(enemy.localScale.x), enemy.localScale.y, enemy.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            player.TakeDamage(1);
            player.Jump(-5, 5);
        }
    }
}
