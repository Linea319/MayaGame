using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public class GamePhaseManager : NetworkBehaviour {
    public Transform playerSpawn;
    public Phase[] phase;
    public LobbyManager netMng;
    float spawnRange = 10f;
    public int phaseCount = 0;


    // Use this for initialization

    public override void OnStartServer()
    {
        base.OnStartServer();
    //NetworkManager.RegisterStartPosition(playerSpawn);
    netMng = FindObjectOfType<LobbyManager>();
        phase[0].StartPhasae();
        phase[0].RpcStartPhase();
    }
	
	// Update is called once per frame
    [ServerCallback]
	void Update () {
	
	}

    [ServerCallback]
    public void NextPhase()
    {
        phaseCount++;
        if(phaseCount >= phase.Length)
        {
            StartCoroutine(Clear());
        }
        else
        {           
            phase[phaseCount].StartPhasae();
            phase[phaseCount].RpcStartPhase();
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

    [Command]
    public void CmdNextPhase()
    {
        NextPhase();
    }
    
}
