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
        lobbyMng.resuls[lobbyMng.gamePlayerNum] = fpsCon.results;
        lobbyMng.gamePlayerNum++;
            fpsCon.weponPath.Clear();
            fpsCon.weponPath.Add(lobby.loadoutPrim);
            fpsCon.weponPath.Add(lobby.loadoutSecond);

            fpsCon.playerName = lobby.playerName;
            fpsCon.conId = lobby.playerId;
    }
}
