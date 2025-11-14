using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    private static SpriteRenderer background1;
    private static Text title1;
    private static Text desc1;

    private static SpriteRenderer background2;
    private static Text title2;
    private static Text desc2;

    private static SpriteRenderer background3;
    private static Text title3;
    private static Text desc3;

    private static SpriteRenderer background4;
    private static Text title4;
    private static Text desc4;

    private static SpriteRenderer background5;
    private static Text title5;
    private static Text desc5;

    private static SpriteRenderer background6;
    private static Text title6;
    private static Text desc6;

    void Start()
    {
        GetAllAchievements(PlayerPrefs.GetInt("PlayerID"));

        background1 = GameObject.Find("Background1").GetComponent<SpriteRenderer>();
        title1 = GameObject.Find("Title1").GetComponent<Text>();
        desc1 = GameObject.Find("Description1").GetComponent<Text>();
        background2 = GameObject.Find("Background2").GetComponent<SpriteRenderer>();
        title2 = GameObject.Find("Title2").GetComponent<Text>();
        desc2 = GameObject.Find("Description2").GetComponent<Text>();
        background3 = GameObject.Find("Background3").GetComponent<SpriteRenderer>();
        title3 = GameObject.Find("Title3").GetComponent<Text>();
        desc3 = GameObject.Find("Description3").GetComponent<Text>();
        background4 = GameObject.Find("Background4").GetComponent<SpriteRenderer>();
        title4 = GameObject.Find("Title4").GetComponent<Text>();
        desc4 = GameObject.Find("Description4").GetComponent<Text>();
        background5 = GameObject.Find("Background5").GetComponent<SpriteRenderer>();
        title5 = GameObject.Find("Title5").GetComponent<Text>();
        desc5 = GameObject.Find("Description5").GetComponent<Text>();
        background6 = GameObject.Find("Background6").GetComponent<SpriteRenderer>();
        title6 = GameObject.Find("Title6").GetComponent<Text>();
        desc6 = GameObject.Find("Description6").GetComponent<Text>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("LevelMenu");
        }
    }

    public void GetAllAchievements(int userId)
    {
        GameManager.Instance.StartCoroutine(GetAllAchievementsCoroutine(userId));
    }

    public static IEnumerator GetAllAchievementsCoroutine(int userId)
    {
        using (UnityWebRequest request = new UnityWebRequest($"https://localhost:7253/api/UsersRecord/getAllRecords/{userId}", "GET"))
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
                AchievementsData achievementsData = JsonUtility.FromJson<AchievementsData>(response);

                if (achievementsData.records == null)
                {
                    background1.color = Color.black;
                    title1.color = Color.black;
                    desc1.text = "???";

                    background2.color = Color.black;
                    title2.color = Color.black;
                    desc2.text = "???";

                    background3.color = Color.black;
                    title3.color = Color.black;
                    desc3.text = "???";

                    background4.color = Color.black;
                    title4.color = Color.black;
                    desc4.text = "???";

                    background5.color = Color.black;
                    title5.color = Color.black;
                    desc5.text = "???";
               
                    background6.color = Color.black;
                    title6.color = Color.black;
                    desc6.text = "???";
                }

                foreach (var recorddata in achievementsData.records)
                {
                    if (recorddata.id_Achievement == 1) Debug.Log(true);
                    else
                    {
                        background1.color = Color.black;
                        title1.color = Color.black;
                        desc1.text = "???";
                    }

                    if (recorddata.id_Achievement == 2) Debug.Log(true);
                    else
                    {
                        background2.color = Color.black;
                        title2.color = Color.black;
                        desc2.text = "???";
                    }

                    if (recorddata.id_Achievement == 3) Debug.Log(true);
                    else
                    {
                        background3.color = Color.black;
                        title3.color = Color.black;
                        desc3.text = "???";
                    }

                    if (recorddata.id_Achievement == 4) Debug.Log(true);
                    else
                    {
                        background4.color = Color.black;
                        title4.color = Color.black;
                        desc4.text = "???";
                    }

                    if (recorddata.id_Achievement == 5) Debug.Log(true);
                    else
                    {
                        background5.color = Color.black;
                        title5.color = Color.black;
                        desc5.text = "???";
                    }

                    if (recorddata.id_Achievement == 6) Debug.Log(true);
                    else
                    {
                        background6.color = Color.black;
                        title6.color = Color.black;
                        desc6.text = "???";
                    }
                }
            }
        }
    }

    [Serializable]
    public class AchievementsData
    {
        public Record[] records;
        public bool status;
    }

    [Serializable]
    public class Record
    {
        public int id_Achievement;
    }
}
