using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private Button musicButton;
    private Button soundButton;

    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    void Start()
    {
        musicButton = GameObject.Find("MusicButton").GetComponent<Button>();
        soundButton = GameObject.Find("SoundButton").GetComponent<Button>();

        musicButton.onClick.AddListener(ControlMusic);
        soundButton.onClick.AddListener(ControlSounds);

        musicButton.image.color = new Color(92, 64, 51);
        soundButton.image.color = new Color(92, 64, 51);

        if (PlayerPrefs.GetInt("Music") == 1)
            musicButton.image.sprite = musicOn;
        else
            musicButton.image.sprite = musicOff;

        if (PlayerPrefs.GetInt("Sound") == 1)
            soundButton.image.sprite = soundOn;
        else
            soundButton.image.sprite = soundOff;
    }
    private void Update()
    {
        Debug.Log(PlayerPrefs.GetInt("Music") + " " + PlayerPrefs.GetInt("Sound"));
    }

    public void ControlMusic()
    {
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            PlayerPrefs.SetInt("Music", 1); 
        }
        else
        {
            PlayerPrefs.SetInt("Music", 0);
        }

        UpdateButtons();
    }
    public void ControlSounds()
    {
        if (PlayerPrefs.GetInt("Sound") == 0)
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 0);
        }

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        if (PlayerPrefs.GetInt("Music") == 1)
            musicButton.image.sprite = musicOn;
        else
            musicButton.image.sprite = musicOff;

        if (PlayerPrefs.GetInt("Sound") == 1)
            soundButton.image.sprite = soundOn;
        else
            soundButton.image.sprite = soundOff;
    }
}
