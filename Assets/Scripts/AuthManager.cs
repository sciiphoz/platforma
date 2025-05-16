using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AuthManager : MonoBehaviour
{
    private Button playButton;

    private InputField loginInput;
    private InputField passwordInput;
    
    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();

        loginInput = GameObject.Find("LoginInput").GetComponent<InputField>();    
        passwordInput = GameObject.Find("PasswordInput").GetComponent<InputField>();

        playButton.onClick.AddListener(CheckPlayer);
    }

    public void CheckPlayer()
    {
        ApiRequests.Login(loginInput.text.Trim(), passwordInput.text.Trim());
    }
}
