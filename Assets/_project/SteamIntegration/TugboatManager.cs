using FishNet;
using FishNet.Managing.Transporting;
using FishNet.Object;
using FishNet.Transporting;
using FishNet.Transporting.Tugboat;
using FishySteamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TugboatManager : NetworkBehaviour
{

    public void OnSteamError(string error)
    {
        UseTugboat();
    }

    public void UseTugboat()
    {
        var man = InstanceFinder.NetworkManager;
        if (man.TryGetComponent<Tugboat>(out var tugboat)
            && man.TryGetComponent<TransportManager>(out var transport)
            && man.TryGetComponent<FishySteamworks.FishySteamworks>(out var fishy))
        {
            fishy.enabled = false;
            tugboat.enabled = true;
            transport.Transport = tugboat;
            //BootstrapManager.Instance.SetTransport(tugboat);
        }
    }
}
