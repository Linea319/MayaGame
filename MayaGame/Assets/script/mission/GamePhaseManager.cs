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
    [SyncVar]
    public int playerCount;
    bool backScene;
    bool goStart = false;
    public int loadCount;
    // Use this for initialization

    public override void OnStartServer()
    {
        base.OnStartServer();
        //NetworkManager.RegisterStartPosition(playerSpawn);
        netMng = FindObjectOfType<LobbyManager>();
        playerCount = netMng.numPlayers;
    }
	
	// Update is called once per frame

    void Start()
    {
        //StartPhase();
        
        //playerCount = playersObj.Length;
        
        //GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>().SetFriend();

    }

    [ServerCallback]
	void Update () {
        if (!goStart)
        {
            if (loadCount >= playerCount) {
                goStart = true;
                StartCoroutine(StartPhase());
            }
            return;
        }

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
    public IEnumerator StartPhase()
    {
        yield return new WaitForSeconds(0.2f);
        backScene = false;
        
        phase[0].StartPhasae();
        phase[0].RpcStartPhase();
        RpcStartPhase();
        GameObject[] playersObj = GameObject.FindGameObjectsWithTag("Player");
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


    [ClientRpc]
    void RpcStartPhase()
    {
        Debug.Log("friend");
        GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>().SetFriend();
    }
    
}
