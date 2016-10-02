using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;

public class setAtachment : MonoBehaviour {
    public string path;
    LobbyPlayerList lobby;
    int slot;
    int num;
    // Use this for initialization

    public void initialize(Transform panel,int cSlot,int atachNum)
    {
       lobby = panel.GetComponent<LobbyPlayerList>();
        slot = cSlot;
        num = atachNum;
    }

    public void Click()
    {
        lobby.SetAttach(path, slot);
        transform.parent.SendMessage("PressButton",num);
        transform.SendMessageUpwards("SetAtachParam", path);
    }
}
