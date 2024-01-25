using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbiesListManagers : MonoBehaviour
{
    [SerializeField] private  GameObject lobbyCardPrefab;
    [SerializeField] private GameObject lobbiesScrollViewContent;
    [SerializeField] private List<GameObject> DisplayedLobbies = new();


    private void Start()
    {
        BootstrapManager.OnLobbyListUpdate += ShowLobbies;
    }

    private void OnDestroy()
    {
        BootstrapManager.OnLobbyListUpdate -= ShowLobbies;

    }

    public void ShowLobbies()
    {
        DestroyLobbies();
        BootstrapManager.Instance.GetLobbiesList();
    }

    private void ShowLobbies(List<CSteamID> lobbies, LobbyDataUpdate_t callback)
    {
        //DestroyLobbies();
        for(var i = 0; i < lobbies.Count; i++)
        {
            if (lobbies[i].m_SteamID == callback.m_ulSteamIDLobby)
            {
                var lobbyCard = Instantiate(lobbyCardPrefab, lobbiesScrollViewContent.transform);
                lobbyCard.GetComponent<LobbyCardManager>().SetData(lobbies[i],
                    SteamMatchmaking.GetLobbyData(lobbies[i], Names.Name));


                DisplayedLobbies.Add(lobbyCard);
            }
        }
    }

    private void DestroyLobbies()
    {
        foreach(var l in DisplayedLobbies)
        {
            Destroy(l);
        }
        DisplayedLobbies.Clear();
    }
}
