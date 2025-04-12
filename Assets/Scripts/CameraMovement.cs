using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 offset;

    private float smoothSpeed = 0.005f;
    private bool isMoving = true;

    private void LateUpdate()
    {
        if (_player.position.x < -6
            || _player.position.x > 30
            || _player.position.y < -5
            || _player.position.y > 5)
            isMoving = false;
        else isMoving = true;

        if (isMoving)
        {
            Vector3 desiredPosition = new Vector3(_player.position.x, _player.position.y, transform.position.z) + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}
