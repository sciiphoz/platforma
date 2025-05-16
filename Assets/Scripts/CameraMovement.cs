using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float topLimitY;
    [SerializeField] private float bottomLimitY;
    [SerializeField] private float leftLimitX;
    [SerializeField] private float rightLimitX;

    private float smoothSpeed = 0.01f;
    private bool isMoving = true;

    private void Update()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            GetComponent<AudioSource>().volume = 0.25f;
        }
        else
        {
            GetComponent<AudioSource>().volume = 0f;
        }
    }

    private void LateUpdate()
    {
        if (_player.position.x < leftLimitX
            || _player.position.x > rightLimitX
            || _player.position.y < bottomLimitY
            || _player.position.y > topLimitY)
            isMoving = false;
        else isMoving = true;

        if (isMoving)
        {
            Vector3 desiredPosition = new Vector3(_player.position.x, _player.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
