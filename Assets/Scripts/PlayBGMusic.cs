using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayBGMusic : MonoBehaviour
{
    [SerializeField] private AudioClip menuBGost;
    [SerializeField] private AudioClip level1BGost;
    [SerializeField] private AudioClip level2BGost;
    [SerializeField] private AudioClip level3BGost;
    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();

        Debug.Log(SceneManager.GetActiveScene().name);

        switch (SceneManager.GetActiveScene().name) 
        {
            case "LevelMenu":
                audioSource.loop = true;
                audioSource.clip = menuBGost;
                audioSource.volume = 0.25f;
                audioSource.Play();
                break;
            case "Level1":
                audioSource.loop = true;
                audioSource.clip = level1BGost;
                audioSource.volume = 0.25f;
                audioSource.Play();
                break;
            case "Level2":
                audioSource.loop = true;
                audioSource.clip = level2BGost;
                audioSource.volume = 0.25f;
                audioSource.Play();
                break;
            case "Level3":
                audioSource.loop = true;
                audioSource.clip = level3BGost;
                audioSource.volume = 0.25f;
                audioSource.Play();
                break;
            default:
                break;
        }
    }
}
