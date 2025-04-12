using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] private float distance;

    private Transform block;
    private bool movingRight = true;
    private Vector3 startPos;

    private Transform player;
    void Start()
    {
        block = GetComponent<Transform>();

        player = GameObject.Find("Player").GetComponent<Transform>();

        startPos = block.position;
    }

    void Update()
    {
        if (block.position.x >= startPos.x + distance)
            movingRight = false;
        else if (block.position.x <= startPos.x - distance)
            movingRight = true;

        if (movingRight)
            block.position = Vector3.MoveTowards(block.position, new Vector3(startPos.x + distance, startPos.y, startPos.z), 0.006f);
        else
            block.position = Vector3.MoveTowards(block.position, new Vector3(startPos.x - distance, startPos.y, startPos.z), 0.006f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.SetParent(gameObject.transform);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.transform.SetParent(null);
    }
}
