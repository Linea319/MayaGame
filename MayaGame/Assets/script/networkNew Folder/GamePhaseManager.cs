using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public class GamePhaseManager : NetworkBehaviour {
    public Transform playerSpawn;
    public Phase[] phase;
    public LobbyManager netMng;
    float spawnRange = 10f;
    [SyncVar]
    public int phaseCount = 0;


    // Use this for initialization

    public override void OnStartServer()
    {
        base.OnStartServer();
    //NetworkManager.RegisterStartPosition(playerSpawn);
    
    }
	
	// Update is called once per frame

    void Start()
    {
        StartPhase();
    }

    [ServerCallback]
	void Update () {
	
	}

    [ServerCallback]
    public void StartPhase()
    {
        netMng = FindObjectOfType<LobbyManager>();
        phase[0].StartPhasae();
        phase[0].RpcStartPhase();
    }

    [ServerCallback]
    public void NextPhase()
    {
        Debug.Log("next" + isServer);
        phaseCount++;
        if(phaseCount >= phase.Length)
        {
            StartCoroutine(Clear());
        }
        else
        {           
            phase[phaseCount].StartPhasae();
            RpcNextPhase(phaseCount);
        }
    }

    [ServerCallback]
    public IEnumerator Clear()
    {
        RpcClear();
        netMng.clear = true;
        yield return new WaitForSeconds(5.0f);
        netMng.GoBackButton();
    }

    [ClientRpc]
    void RpcClear()
    {
        GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>().SetTaskText("Task Complete");
    }

    [ClientRpc]
    void RpcNextPhase(int slot)
    {
        phase[slot].StartPhasae();
    }

    [Command]
    public void CmdNextPhase()
    {
        NextPhase();
    }
    
}
