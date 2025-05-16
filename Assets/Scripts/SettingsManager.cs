using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    private Button musicButton;
    private Button soundButton;
    private Button menuButton;
    private Button restartButton;

    [SerializeField] private Sprite musicOn;
    [SerializeField] private Sprite musicOff;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    void Start()
    {
        musicButton = GameObject.Find("MusicButton").GetComponent<Button>();
        soundButton = GameObject.Find("SoundButton").GetComponent<Button>();
        menuButton = GameObject.Find("Menu").GetComponent<Button>();
        restartButton = GameObject.Find("Restart").GetComponent<Button>();

        musicButton.onClick.AddListener(ControlMusic);
        soundButton.onClick.AddListener(ControlSounds);
        menuButton.onClick.AddListener(OpenMenu);
        restartButton.onClick.AddListener(RestartLevel);

        if (PlayerPrefs.GetInt("Music") == 1)
            musicButton.image.sprite = musicOn;
        else
            musicButton.image.sprite = musicOff;

        if (PlayerPrefs.GetInt("Sound") == 1)
            soundButton.image.sprite = soundOn;
        else
            soundButton.image.sprite = soundOff;

        musicButton.image.color = new Color(92, 64, 51);
        soundButton.image.color = new Color(92, 64, 51);
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

    public void OpenMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

        musicButton.image.color = new Color(92, 64, 51);
        soundButton.image.color = new Color(92, 64, 51);
    }
}
