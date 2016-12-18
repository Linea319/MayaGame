using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class doorOpen : NetworkBehaviour {
    [SyncVar]
    bool open = false;
    [SyncVar]
    bool openStart = false;
    Collider mycol;
    public Animator anim;
    public float openTime = 0f;
    [SyncVar]
    float timer = 0;
    public GameObject openObject;
    FPS_UI ui;
	// Use this for initialization
	void Start () {
        mycol = GetComponent<Collider>();
        ui = GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>();
    }
	
    
    public void Open()
    {
        CmdOpen();
    } 

    [Command]
    void CmdOpen()
    {
        if (openTime <= 0)
        {
            open = true;
            RpcOpen();
        }
        else
        {
            timer = Time.time + openTime;
            openStart = true;
            RpcOpenStart();
        }
    }
    [ClientRpc]
    void RpcOpen()
    {
        mycol.enabled = false;
        string[] msg = new string[2];
        ui.SetTaskInfo(msg);
    }

    [ClientRpc]
    void RpcOpenStart()
    {
        if(openObject != null)
        {
            openObject.SetActive(true);
        }
        GetComponent<UIMessenger>().enabled = false;       
    }

    // Update is called once per frame
    void Update () {
        if (isClient)
        {
            anim.SetBool("open", open);
            if(openStart && Time.time < timer)
            {
                string[] msg = new string[2];
                msg[0] = "Door Hacking...";
                msg[1] = (timer - Time.time).ToString("f1");
                ui.SetTaskInfo(msg);
            }
        }
        if (isServer)
        {
            if (openStart && Time.time > timer)
            {
                open = true;
                RpcOpen();
            }
        }
	}
}
