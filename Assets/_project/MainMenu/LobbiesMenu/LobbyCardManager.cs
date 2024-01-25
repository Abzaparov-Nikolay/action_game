using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCardManager : MonoBehaviour
{

    //public static Action<CSteamID> OnJoinClick;
    //public string name;
    //public Button joinButton;

    public TextMeshProUGUI nameText;
    public CSteamID steamID;

    private void OnEnable()
    {
        //joinButton.onClick.AddListener(OnJoinButtonClick);
    }

    private void OnDisable()
    {
        //joinButton.onClick.RemoveListener(OnJoinButtonClick);
    }

    public void SetData(CSteamID id, string name)
    {
        steamID = id;
        nameText.text = name;
    }

    public void JoinLobby()
    {
        BootstrapManager.JoinById(steamID);

    }

    private void OnJoinButtonClick()
    {
        JoinLobby();
    }
}
