using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Phase_Hack : Phase {
    public UIMessenger[] message;
    //public Renderer serverRender;
    public Color startColor;
    public Color endColor;
    public float hackTime;
    public float stopTime;
    public float stopRate;
    [SyncVar]
    public bool hack = false;
    float timer;
    [SyncVar]
    public float hackTimer;
    FPS_UI uicon;
    [SyncVar]
    int cMessage=0;

    // Update is called once per frame
    void Start()
    {
        uicon = GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>();
    }

    public override void StartPhasae()
    {
        base.StartPhasae();
        message[0].enabled = true;
        timer = stopTime+Time.time;
        hackTimer = hackTime;

    }

    public void Hack()
    {
        if (isClient)
        {
            CmdHack();
            Debug.Log("HAck");
        }
    }

    [Command]
    public void CmdHack()
    {
        hack = true;
        RpcHack();
        Debug.Log("comhack");
        timer = stopTime + Time.time;

    }
    [ClientRpc]
    void RpcHack()
    {
        message[cMessage].enabled = false;
        Debug.Log("rpchack");
    }

    [ClientRpc]
    void RpcStop()
    {
        message[cMessage].enabled = true;
    }

    void Update()
    {
        if (hack)
        {
            if (isServer)
            {
                hackTimer -= Time.deltaTime;
                if (timer < Time.time)
                {
                    if (Random.Range(0, 100) < stopRate)
                    {
                        cMessage = Random.Range(0, message.Length);
                        RpcStop();
                        hack = false;
                    }
                    timer = stopTime + Time.time;
                }




                if (hackTimer <= 0)
                {
                    ClearPhase();

                }
            }
            if(isClient)
            {
                //Debug.Log("clienthack");
                string[] msg = new string[2];
                msg[0] = "Server Hacking...";
                msg[1] = (hackTimer).ToString("f1");
                if (hackTimer <= 0)
                {
                    msg[0] = "";
                    msg[1] = "";
                }
                uicon.SetTaskInfo(msg);
            }
        }
    }
}
