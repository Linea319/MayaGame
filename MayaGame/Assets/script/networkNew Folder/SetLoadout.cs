using UnityEngine;
using System.Collections;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class SetLoadout : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        FPSController fpsCon = gamePlayer.GetComponent<FPSController>();
        LobbyManager lobbyMng = GetComponent<LobbyManager>();
        Debug.Log("scenechange");

        lobbyMng.gamePlayerObject[lobbyMng.gamePlayerNum] = gamePlayer;
        fpsCon.results = lobbyMng.resuls[lobbyMng.gamePlayerNum];
        lobbyMng.gamePlayerNum++;
        fpsCon.weponPath.Clear();
        fpsCon.weponPath.Add(lobby.loadoutPrim);
        fpsCon.weponPath.Add(lobby.primAtach1);
        fpsCon.weponPath.Add(lobby.primAtach2);
        fpsCon.weponPath.Add(lobby.loadoutSecond);
        fpsCon.weponPath.Add(lobby.secondAtach1);
        fpsCon.weponPath.Add(lobby.secondAtach2);
        fpsCon.weponPath.Add(lobby.item);

        fpsCon.playerName = lobby.playerName;
        fpsCon.conId = lobby.playerId;
    }
}
