using FishNet.Managing.Scened;
using FishNet.Object;
using HeathenEngineering.SteamworksIntegration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapNetworkManager : NetworkBehaviour
{
    private static BootstrapNetworkManager instance;

    public static BootstrapNetworkManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("BNM already exists");
        }
        instance = this;
    }

    public static void ChangeNetworkScene(string sceneName, string[] scenesToClose)
    {
        //instance.CloseScenes(scenesToClose);


        SceneLoadData data = new SceneLoadData(sceneName);
        SceneUnloadData sud = new SceneUnloadData(scenesToClose);

        var conns = instance.ServerManager.Clients.Values.ToArray();
        instance.SceneManager.UnloadConnectionScenes(conns, sud);
        //instance.SceneManager.Sc(conns, sud);
        instance.SceneManager.OnLoadEnd += piss;
        instance.SceneManager.LoadConnectionScenes(conns, data);
    }

    private static void piss(SceneLoadEndEventArgs e)
    {

        if (e.LoadedScenes.Count() != 0 && e.LoadedScenes.FirstOrDefault() != null)
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(e.LoadedScenes.FirstOrDefault());
    }

    [ServerRpc(RequireOwnership = false)]
    private void CloseScenes(string[] scenes)
    {
        CloseScenesObserver(scenes);
    }

    [ObserversRpc]
    private void CloseScenesObserver(string[] scenes)
    {
        foreach (var sceneName in scenes)
        {
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
        }
    }
}
