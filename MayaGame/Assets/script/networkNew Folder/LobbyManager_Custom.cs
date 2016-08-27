using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class LobbyManager_Custom :NetworkLobbyManager {
    public InputField IPinput;
    bool host;

    void Update()
    {
        Debug.Log(isNetworkActive);
    }

    public void ButtonHost()
    {
        host = true;
        Debug.Log("HOST");
        SetPort();
        //NetworkManager.singleton.StartHost();
        StartHost();
    }

    public void ButtonConnect()
    {
       
        SetPort(); 
        string ipAddress = IPinput.text;
        Debug.Log(ipAddress);
        NetworkManager.singleton.networkAddress = ipAddress;
        NetworkManager.singleton.StartClient();
    }

    public void ButtonDisConnect()
    {
        if (host)
        {
            StopHost();
        }
        else
        {
            StopClient();
        }
        host = false;

    }

    public override void OnLobbyStartClient(NetworkClient lobbyClient)
    {
        base.OnLobbyStartClient(lobbyClient);
    }

    public override void OnLobbyStartHost()
    {
        base.OnLobbyStartHost();
    }

    void SetUI_Lobby()
    {

    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }
}
