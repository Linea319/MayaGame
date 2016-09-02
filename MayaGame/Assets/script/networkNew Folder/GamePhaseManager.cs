﻿using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public class GamePhaseManager : NetworkBehaviour {
    public Transform playerSpawn;
    public Phase[] phase;
    public NetworkLobbyManager netMng;
    float spawnRange = 10f;
    int phaseCount = 0;


	// Use this for initialization
    [Server]
	void Start () {
        //NetworkManager.RegisterStartPosition(playerSpawn);
        netMng = FindObjectOfType<NetworkLobbyManager>();
        phase[0].StartPhasae();
	}
	
	// Update is called once per frame
    [Server]
	void Update () {
	
	}

    [Server]
    public void NextPhase()
    { phaseCount++;
        if(phaseCount >= phase.Length)
        {
            StartCoroutine( Clear());
        }
    }

    [Server]
    public IEnumerator Clear()
    {
        RpcClear();
        yield return new WaitForSeconds(2.0f);
        netMng.ServerReturnToLobby();
    }

    [ClientRpc]
    void RpcClear()
    {
        GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>().SetTaskText("Task Complete");
    }
    
}