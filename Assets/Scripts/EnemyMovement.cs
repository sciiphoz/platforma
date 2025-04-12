using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float distance;
    private Transform enemy;
    private Vector3 startPos;
    private bool isRight = true;
    void Start()
    {
        enemy = GetComponent<Transform>();
        startPos = enemy.position;
    }

    void Update()
    {
        if (enemy.position.x >= startPos.x + distance)
            isRight = false;
        else if (enemy.position.x <= startPos.x - distance)
            isRight = true;

        if (isRight)
            enemy.position = Vector3.MoveTowards(enemy.position, new Vector3(startPos.x + distance, startPos.y, startPos.z), 0.006f);
        else
            enemy.position = Vector3.MoveTowards(enemy.position, new Vector3(startPos.x - distance, startPos.y, startPos.z), 0.006f);
    }
}
