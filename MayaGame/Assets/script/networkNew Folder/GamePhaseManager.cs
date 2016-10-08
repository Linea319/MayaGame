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
    HitManagerPlayer[] players = new HitManagerPlayer[4];
    int playerCount;
    bool backScene;

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
        int count = 0;
        for(int i = 0; i < playerCount; i++)
        {
           
            if(players[i].hitPoint <= 0)
            {
                count++;
            }
        }
        if(count >= playerCount && !backScene)
        {
            backScene = true;
            netMng.GoBackButton();
        }

	}

    [ServerCallback]
    public void StartPhase()
    {
        backScene = false;
        netMng = FindObjectOfType<LobbyManager>();
        phase[0].StartPhasae();
        phase[0].RpcStartPhase();

        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
        playerCount = playersObj.Length;
        for (int j = 0; j < playersObj.Length; j++)
        {
            players[j] = playersObj[j].GetComponent<HitManagerPlayer>();
        }

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
