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

    private Slider musicVolumeSlider;
    private Slider soundVolumeSlider;

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

        musicVolumeSlider = GameObject.Find("MusicSlider").GetComponent<Slider>();
        soundVolumeSlider = GameObject.Find("SoundSlider").GetComponent<Slider>();

        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music");
        soundVolumeSlider.value = PlayerPrefs.GetFloat("Sound");

        musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        soundVolumeSlider.onValueChanged.AddListener(ChangeSoundVolume);

        musicButton.onClick.AddListener(ControlMusic);
        soundButton.onClick.AddListener(ControlSounds);
        menuButton.onClick.AddListener(OpenMenu);
        restartButton.onClick.AddListener(RestartLevel);

        if (PlayerPrefs.GetFloat("Music") != 0f)
            musicButton.image.sprite = musicOn;
        else
            musicButton.image.sprite = musicOff;

        if (PlayerPrefs.GetFloat("Sound") != 0f)
            soundButton.image.sprite = soundOn;
        else
            soundButton.image.sprite = soundOff;

        musicButton.image.color = new Color(92, 64, 51);
        soundButton.image.color = new Color(92, 64, 51);
    }

    public void ControlMusic()
    {
        if (PlayerPrefs.GetFloat("Music") == 0f)
        {
            PlayerPrefs.SetFloat("Music", 1f);
        }
        else
        {
            musicVolumeSlider.value = 0f;
            PlayerPrefs.SetFloat("Music", 0f);
        }

        UpdateButtons();
    }
    public void ControlSounds()
    {
        if (PlayerPrefs.GetFloat("Sound") == 0f)
        {
            PlayerPrefs.SetFloat("Sound", 1f);
        }
        else
        {
            soundVolumeSlider.value = 0f;
            PlayerPrefs.SetFloat("Sound", 0f);
        }

        UpdateButtons();
    }

    public void ChangeMusicVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("Music", sliderValue);

        UpdateButtons();
    }

    public void ChangeSoundVolume(float sliderValue)
    {
        PlayerPrefs.SetFloat("Sound", sliderValue);

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
        if (PlayerPrefs.GetFloat("Music") != 0f)
            musicButton.image.sprite = musicOn;
        else
            musicButton.image.sprite = musicOff;

        if (PlayerPrefs.GetFloat("Sound") != 0f)
            soundButton.image.sprite = soundOn;
        else
            soundButton.image.sprite = soundOff;

        musicButton.image.color = new Color(92, 64, 51);
        soundButton.image.color = new Color(92, 64, 51);

        UpdateSliders();
    }

    private void UpdateSliders()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music");
        soundVolumeSlider.value = PlayerPrefs.GetFloat("Sound");
    }
}
