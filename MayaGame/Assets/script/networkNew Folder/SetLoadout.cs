using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class SetLoadout : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        FPSController fpsCon = gamePlayer.GetComponent<FPSController>();
        fpsCon.weponPath.Clear();
        fpsCon.weponPath.Add(lobby.loadoutPrim);
        fpsCon.weponPath.Add(lobby.loadoutSecond);
        
    }
}
