using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Phase_Correct : Phase {
    public int needBagnum;
    [SyncVar]
    int bagnum = 0;
    FPS_UI uicon;

	// Use this for initialization
	void Start () {
        uicon = GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>();
    }

    public override void StartPhasae()
    {
        base.StartPhasae();
        RpcUpdateBag();
    }

    // Update is called once per frame
    [ServerCallback]
	void Update () {
		if(bagnum >= needBagnum)
        {
            ClearPhase();
        }
	}

    [ServerCallback]
    public void UpdateBag()
    {
        bagnum++;
        RpcUpdateBag();
    }

    [ClientRpc]
    void RpcUpdateBag()
    {
        string[] msg = new string[2];
        msg[0] = "Correct status";
        msg[1] = bagnum.ToString()+"/"+needBagnum.ToString();
        uicon.SetTaskInfo(msg);
    }

}
