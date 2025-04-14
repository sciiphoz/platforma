using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    private Transform text;

    private Transform logo;
    private Vector3 logoStartPos;
    private bool isUp = true;
    private float logoDistance = 1f;

    private Vector3 velocity;

    void Start()
    {
        text = GameObject.Find("PlayText").GetComponent<Transform>();

        logo = GameObject.Find("LogoUI").GetComponent<Transform>();
        logoStartPos = logo.transform.position;

        StartCoroutine(TextCoroutine());
    }

    void Update()
    {
        if (logo.position.y >= logoStartPos.y + logoDistance - 0.1f)
            isUp = false;
        else if (logo.position.y <= logoStartPos.y - logoDistance + 0.1f) isUp = true;

        if (isUp)
            logo.position = Vector3.SmoothDamp(logo.position, new Vector3(logo.position.x, logoStartPos.y + logoDistance, logo.position.z), ref velocity, 2f);
        else
            logo.position = Vector3.SmoothDamp(logo.position, new Vector3(logo.position.x, logoStartPos.y - logoDistance, logo.position.z), ref velocity, 2f);

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("LevelMenu");
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {

        }
    }

    public IEnumerator TextCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.75f);
            text.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.75f);
            text.gameObject.SetActive(true);
        }
    }
}
