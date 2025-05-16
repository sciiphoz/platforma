using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fall : MonoBehaviour
{
    private PlayerMovement _player; 
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<PlayerMovement>();    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("a");

            _player.TakeDamage(3, true);
        }
    }
}
