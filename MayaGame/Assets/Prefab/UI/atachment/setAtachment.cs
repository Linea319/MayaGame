using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;

public class setAtachment : MonoBehaviour {
    public string path;
    LobbyPlayerList lobby;
    int slot;
    // Use this for initialization

    public void initialize(Transform panel,int cSlot)
    {
       lobby = panel.GetComponent<LobbyPlayerList>();
        slot = cSlot;
    }

    public void Click()
    {
        lobby.SetAttach(path, slot);
    }
}
