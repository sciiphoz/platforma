using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private PlayerMovement player;

    private Transform axe;
    private bool isRight = true;
    private float timeCount = 0f;
    void Start()
    {
        axe = GetComponent<Transform>();
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (isRight)
        {
            axe.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, 75), Quaternion.Euler(0, 0, -75), timeCount);
            timeCount += Time.deltaTime;
            
            if (axe.rotation == Quaternion.Euler(0, 0, -75))
            {
                isRight = false;
                timeCount = 0f;
            }
        }
        else
        {
            axe.rotation = Quaternion.Slerp(Quaternion.Euler(0, 0, -75), Quaternion.Euler(0, 0, 75), timeCount);
            timeCount += Time.deltaTime;

            if (axe.rotation == Quaternion.Euler(0, 0, 75))
            {
                isRight = true;
                timeCount = 0f;
            }
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
