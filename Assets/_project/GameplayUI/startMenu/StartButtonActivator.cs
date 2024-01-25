using FishNet.Connection;
using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButtonActivator : NetworkBehaviour
{
    [SerializeField] private Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        SpawnerPlayers.OnSpawned += DeactivateButton;
    }

    private void OnDestroy()
    {
        SpawnerPlayers.OnSpawned -= DeactivateButton;
    }

    void DeactivateButton(NetworkObject conn)
    {
        deactivateOnClient(conn);
    }

    [ObserversRpc]
    void deactivateOnClient(NetworkObject conn)
    {
        if(conn.IsOwner)
        {
            //startButton.enabled = false;
            this.gameObject.SetActive(false);
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        this.gameObject.SetActive(true);
    }
}
