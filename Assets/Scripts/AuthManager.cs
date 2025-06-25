using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    private Button loginButton;
    private Button registerButton;

    private Text loginInput;
    private Text passwordInput;
    private static Text error;
    
    void Start()
    {
        loginButton = GameObject.Find("AuthButton").GetComponent<Button>();
        registerButton = GameObject.Find("RegButton").GetComponent<Button>();

        loginInput = GameObject.Find("LoginText").GetComponent<Text>();    
        passwordInput = GameObject.Find("PasswordText").GetComponent<Text>();
        error = GameObject.Find("ErrorText").GetComponent<Text>();

        loginButton.onClick.AddListener(LoginClick);
        registerButton.onClick.AddListener(RegisterClick);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && error.text != String.Empty) error.text = String.Empty;
    }

    public void LoginClick()
    {
        Login(loginInput.text.Trim(), passwordInput.text.Trim());
    }

    public void RegisterClick()
    {
        Register(loginInput.text.Trim(), passwordInput.text.Trim());
    }

    public static void Register(string login, string password)
    {
        GameManager.Instance.StartCoroutine(RegisterCoroutine(login, password));
    }
    public static IEnumerator RegisterCoroutine(string login, string password)
    {
        var data = new LoginData { login = login, password = password };
        var jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest registerRequest = new UnityWebRequest("https://localhost:7253/api/UsersLogins/user/register", "POST");

        registerRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
        registerRequest.downloadHandler = new DownloadHandlerBuffer();
        registerRequest.SetRequestHeader("Content-Type", "application/json");

        yield return registerRequest.SendWebRequest();

        if (registerRequest.result == UnityWebRequest.Result.ConnectionError ||
            registerRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Registration failed: " + registerRequest.error);
            error.text = "Registration failed.";
            yield break;
        }

        Debug.Log("Registration response: " + registerRequest.downloadHandler.text);

        try
        {
            LoginResponse userData = JsonUtility.FromJson<LoginResponse>(registerRequest.downloadHandler.text);
            if (userData != null)
            {
                PlayerPrefs.SetInt("PlayerID", userData.user.id_User);
                PlayerPrefs.SetInt("Level1Score", userData.user.level1score);
                PlayerPrefs.SetInt("Level2Score", userData.user.level2score);
                PlayerPrefs.Save();
                SceneManager.LoadScene("MainMenu");
            }
            else
            {
                Debug.LogError("Failed to parse user data after registration");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("JSON parse error after registration: " + ex.Message);
        }
    }
    public static void Login(string login, string password)
    {
        Debug.Log(login + password);
        GameManager.Instance.StartCoroutine(LoginCoroutine(login, password));
    }
    public static IEnumerator LoginCoroutine(string login, string password)
    {
        var data = new LoginData { login = login, password = password };
        var jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        Debug.Log("Sending login data: " + jsonData);

        UnityWebRequest request = new UnityWebRequest("https://localhost:7253/api/UsersLogins/user/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.LogError("Login failed: " + request.error);

            error.text = "Login failed.";
        }

        else
        {
            Debug.Log("Login response: " + request.downloadHandler.text);

            try
            {
                LoginResponse userData = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                if (userData != null)
                {
                    PlayerPrefs.SetInt("PlayerID", userData.user.id_User);
                    PlayerPrefs.SetInt("Level1Score", userData.user.level1score);
                    PlayerPrefs.SetInt("Level2Score", userData.user.level2score);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    Debug.LogError("Failed to parse user data after login");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("JSON parse error after login: " + ex.Message);
            }
        }
    }
}
