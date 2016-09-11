using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public struct ResultParam
{
    public string name;
    public int kill;
    public int down;
    public int shoot;
    public int hit;
};

public class ResultPanel : NetworkBehaviour {
    public RectTransform playerList;
    public GameObject resultPrefab;
    [SyncVar]
    int playernum = 0;
    ResultParam[] player = new ResultParam[4];
    
    string[] playerNames = new string[4];

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public void AddPlayer(string name, ResultParam param)
    {
        if(playernum < 4)
        playernum++;
        playerNames[playernum - 1] = name;
        player[playernum - 1] = param;
        SetPlayerOne();
    }

    public void SetPlayer()
    {
        Debug.Log("set");
        for(int i = 0; i < playernum; i++)
        {
            GameObject obj = Instantiate(resultPrefab);
            obj.GetComponent<RectTransform>().SetParent(playerList,false);
            obj.GetComponent<ResultPlayer>().SetParam(playerNames[i], player[i]);
        }
    }

    public void SetPlayerOne()
    {
        GameObject obj = Instantiate(resultPrefab);
        obj.GetComponent<RectTransform>().SetParent(playerList, false);
        obj.GetComponent<ResultPlayer>().SetParam(playerNames[playernum - 1], player[playernum - 1]);
    }

    [ServerCallback]
    public void Add(ResultParam result)
    {
        Debug.Log("add"+isServer);
        RpcAddResult(result);
        AddPlayer(result.name, result);
    }
    
    //rpc
    [ClientRpc]
    void RpcAddResult(ResultParam result)
    {
        Debug.Log("addrpc");
        AddPlayer(result.name, result);

    }
}
