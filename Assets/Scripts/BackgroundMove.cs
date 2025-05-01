using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    private Transform background;
    private Vector3 backgroundStartPos;
    private bool isRight = true;
    private float distance = 10.8f;

    private Vector3 velocity;
    void Start()
    {
        background = GameObject.Find("BackgroundLayer").GetComponent<Transform>();
        backgroundStartPos = background.transform.position;
    }
    void Update()
    {
        if (background.position.x >= backgroundStartPos.x + distance - 0.1f)
            isRight = false;
        else if (background.position.x <= backgroundStartPos.x - distance + 0.1f) isRight = true;

        if (isRight)
            background.position = Vector3.SmoothDamp(background.position, new Vector3(background.position.x + distance, backgroundStartPos.y, background.position.z), ref velocity, 12f);
        else
            background.position = Vector3.SmoothDamp(background.position, new Vector3(background.position.x - distance, backgroundStartPos.y, background.position.z), ref velocity, 12f);

    }
}
