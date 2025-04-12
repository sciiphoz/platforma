using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarManager : MonoBehaviour
{
    private PlayerMovement player;
    private Transform star;

    private Vector3 starStartPos;
    private Vector3 velocity;
    private float distance = 1;
    private bool isUp;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        star = GetComponent<Transform>();
    }
    void Update()
    {
        if (star.position.y >= starStartPos.y + distance - 0.1f)
            isUp = false;
        else if (star.position.y <= starStartPos.y - distance + 0.1f) isUp = true;
    
        if (isUp)
            star.position = Vector3.SmoothDamp(star.position, new Vector3(star.position.x, starStartPos.y + distance, star.position.z), ref velocity, 2f);
        else
            star.position = Vector3.SmoothDamp(star.position, new Vector3(star.position.x, starStartPos.y - distance, star.position.z), ref velocity, 2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
            player.AddStars();
    }
}
