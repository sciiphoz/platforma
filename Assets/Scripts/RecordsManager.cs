using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecordsManager : MonoBehaviour
{
    private static Text recordsText;

    void Start()
    {
        recordsText = GameObject.Find("RecordsData").GetComponent<Text>();

        GetAllUsers();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LevelMenu");
        }
    }

    public void GetAllUsers()
    {
        GameManager.Instance.StartCoroutine(GetAllUsersCoroutine());
    }

    public static IEnumerator GetAllUsersCoroutine()
    {
        using (UnityWebRequest request = new UnityWebRequest("https://localhost:7253/api/UsersLogins/getAllUsers", "GET"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();

            if (request.error != null)
            {
                Debug.Log(request.error);
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log(response);
                UsersData usersdata = JsonUtility.FromJson<UsersData>(response);

                foreach (var userdata in usersdata.users)
                {
                    recordsText.text += userdata.login + " — " 
                        + (userdata.level1score + userdata.level2score + userdata.level3score) + $"\n";
                }
            }
        }
    }

    [Serializable]
    public class UsersData
    {
        public UserData[] users;
        public bool status;
    }

    [Serializable]
    public class UserData
    {
        public int id_User;
        public string login;
        public string password;
        public int level1score;
        public int level2score;
        public int level3score;
    }
}
