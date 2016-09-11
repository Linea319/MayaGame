using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResultPlayer : MonoBehaviour {
    public Text namePanel;
    public Text killPanel;
    public Text downPanel;
    public Text accuracyPanel;
    string playerName;
    int killCount;
    int downCount;
    int accuracy;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetParam(string name,ResultParam param)
    {
        playerName = name;
        killCount = param.kill;
        downCount = param.down;
        accuracy = param.shoot;
        SetText();
    }

    void SetText()
    {
        namePanel.text = playerName;
        killPanel.text = killCount.ToString();
        downPanel.text = downCount.ToString();
        accuracyPanel.text = accuracy.ToString();
    }

    
}
