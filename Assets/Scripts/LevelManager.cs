using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform level2lock;
    [SerializeField] private Transform level3lock;

    private Button level1Button;
    private Button level2Button;
    private Button level3Button;

    private Button recordsButton;
    void Start()
    {
        level1Button = GameObject.Find("Level1Button").GetComponent<Button>();
        level2Button = GameObject.Find("Level2Button").GetComponent<Button>();
        level3Button = GameObject.Find("Level3Button").GetComponent<Button>();

        recordsButton = GameObject.Find("RecordsButton").GetComponent<Button>();

        level1Button.onClick.AddListener(OpenLevel1);
        level2Button.onClick.AddListener(OpenLevel2);
        level3Button.onClick.AddListener(OpenLevel3);

        recordsButton.onClick.AddListener(OpenRecords);

        if (PlayerPrefs.GetInt("Level1Score") == 0)
        {
            level2lock.gameObject.SetActive(true);

            level2Button.enabled = false;
        }

        if (PlayerPrefs.GetInt("Level2Score") == 0)
        {
            level3lock.gameObject.SetActive(true);

            level3Button.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void OpenLevel1()
    {
        SceneManager.LoadScene("Level1");
    }
    public void OpenLevel2()
    {
        SceneManager.LoadScene("Level2");
    }
    public void OpenLevel3()
    {
        SceneManager.LoadScene("Level3");
    }
    public void OpenRecords()
    {
        SceneManager.LoadScene("RecordsScene");
    }
}
