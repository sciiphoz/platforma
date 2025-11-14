using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts
{
    public class ApiRequests
    {
        private static string apiUrl = "https://localhost:7253/api/UsersLogins/";

        public static void AddScore(int userId, int amount, int lvl)
        {
            GameManager.Instance.StartCoroutine(AddScoreAsync(userId, amount, lvl));
        }
        public static IEnumerator AddScoreAsync(int userId, int amount, int lvl)
        {
            using (UnityWebRequest request = new UnityWebRequest(apiUrl + $"user/{userId}/score/{amount}/level/{lvl}", "PUT"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                if (request.error != null)
                {
                    Debug.Log(request.error);
                }
            }
        }
        public static IEnumerator AddAchievementAsync(int achievementId, int userId)
        {
            using (UnityWebRequest request = new UnityWebRequest($"https://localhost:7253/api/UsersRecord/achievement/{achievementId}/addTo/{userId}", "POST"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                yield return request.SendWebRequest();

                if (request.error != null)
                {
                    Debug.Log(request.error);
                }
            }
        }

        public static void GetUser(int userId)
        {
            GameManager.Instance.StartCoroutine(GetUserCoroutine(userId));
        }
        public static IEnumerator GetUserCoroutine(int userId)
        {
            using (UnityWebRequest request = UnityWebRequest.Get(apiUrl + $"getUser/{userId}"))
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
                    LoginResponse userData = JsonUtility.FromJson<LoginResponse>(response);

                    PlayerPrefs.SetInt("Level1Score", userData.user.level1score);
                    PlayerPrefs.SetInt("Level2Score", userData.user.level2score);
                    PlayerPrefs.Save();
                }
            }
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

            UnityWebRequest registerRequest = new UnityWebRequest(apiUrl + "user/register", "POST");

            registerRequest.uploadHandler = new UploadHandlerRaw(bodyRaw);
            registerRequest.downloadHandler = new DownloadHandlerBuffer();
            registerRequest.SetRequestHeader("Content-Type", "application/json");

            yield return registerRequest.SendWebRequest();

            if (registerRequest.result == UnityWebRequest.Result.ConnectionError ||
                registerRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Registration failed: " + registerRequest.error);
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
                    SceneManager.LoadScene("LevelMenu");
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

            UnityWebRequest request = new UnityWebRequest(apiUrl + "user/login", "POST");
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.error != null)
            {
                Debug.LogError("Login failed: " + request.error);

                Register(login, password);
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
                        SceneManager.LoadScene("LevelMenu");
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


    [System.Serializable]
    public class LoginResponse
    {
        public UserData user;
        public bool status;
    }

    [System.Serializable]
    public class UserData
    {
        public int id_User;
        public string login;
        public string password;
        public int level1score;
        public int level2score;
        public int level3score;
    }

    [System.Serializable]
    public class LoginData
    {
        public string login;
        public string password;
    }

    [System.Serializable]
    public class UserSkin
    {
        public int id_User_Skin;
        public int id_User;
        public int id_Skin;
    }

    [System.Serializable]
    public class SkinsData
    {
        public UserSkin[] userskin;
        public bool status;
    }
}
