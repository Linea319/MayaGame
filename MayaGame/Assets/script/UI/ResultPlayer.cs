using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class ResultPlayer : MonoBehaviour {
    public Text namePanel;
    public Text killPanel;
    public Text downPanel;
    public Text accuracyPanel;
   // [SyncVar]
    string playerName;
    //[SyncVar]
    int killCount;
   // [SyncVar]
    int downCount;
   // [SyncVar]
    int accuracy;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //[Server]
    public void SetParam(string name,ResultParam param)
    {
        playerName = name;
        killCount = param.kill;
        downCount = param.down;
        accuracy = param.shoot;
        namePanel.text = playerName;
        killPanel.text = killCount.ToString();
        downPanel.text = downCount.ToString();
        accuracyPanel.text = accuracy.ToString();
        RpcSetText();
    }

    //[ClientRpc]
    void RpcSetText()
    {
        Debug.Log("text");
        namePanel.text = playerName;
        killPanel.text = killCount.ToString();
        downPanel.text = downCount.ToString();
        accuracyPanel.text = accuracy.ToString();
    }

    
}
