using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Prototype.NetworkLobby;
using System.Collections;

public struct ResultParam
{
    public string name;
    public string mission;
    public bool clear;
    public int kill;
    public int down;
    public int shoot;
    public int hit;
};

public class ResultPanel : MonoBehaviour {
    public RectTransform playerList;
    public Text missionText;
    public Text ClearText;

    public GameObject resultPrefab;
    int playernum = 0;
    ResultParam[] player = new ResultParam[4];
    GameObject[] resultObj = new GameObject[4];
    string[] playerNames = new string[4];
    int count=0;
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
            NetworkServer.Spawn(obj);
            obj.GetComponent<ResultPlayer>().SetParam(playerNames[i], player[i]);
        }
    }

    public void SetPlayerOne()
    {
        GameObject obj = Instantiate(resultPrefab);
        obj.GetComponent<RectTransform>().SetParent(playerList, false);
        obj.GetComponent<ResultPlayer>().SetParam(playerNames[playernum - 1], player[playernum - 1]);
        resultObj[count] = obj;
        count++;
    }

    public void Add(ResultParam result)
    {
        Debug.Log("add");
        AddPlayer(result.name, result);
        missionText.text = "MISSION:"+result.mission;
        if (result.clear)
        {
            ClearText.text = "CLEAR";
            ClearText.color = Color.cyan;
        }
        else
        {
            ClearText.text = "ABORT";
            ClearText.color = Color.red;
        }
           
    }

    public void Reset()
    {
        playernum = 0;
        for (int i = 0; i < 4; i++)
        {
            if (resultObj[i] != null) Destroy(resultObj[i]);
        }


    }
    
}
