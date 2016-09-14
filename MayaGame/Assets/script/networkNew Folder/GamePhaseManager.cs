using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public class GamePhaseManager : NetworkBehaviour {
    public Transform playerSpawn;
    public Phase[] phase;
    public LobbyManager netMng;
    float spawnRange = 10f;
    int phaseCount = 0;


	// Use this for initialization
    [ServerCallback]
	void Start () {
        //NetworkManager.RegisterStartPosition(playerSpawn);
        netMng = FindObjectOfType<LobbyManager>();
        phase[0].StartPhasae();
	}
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
	
	}

    [ServerCallback]
    public void NextPhase()
    { phaseCount++;
        if(phaseCount >= phase.Length)
        {
            StartCoroutine( Clear());
        }
    }

    [ServerCallback]
    public IEnumerator Clear()
    {
        RpcClear();
        netMng.clear = true;
        yield return new WaitForSeconds(2.0f);
        netMng.GoBackButton();
    }

    [ClientRpc]
    void RpcClear()
    {
        GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>().SetTaskText("Task Complete");
    }
    
}
