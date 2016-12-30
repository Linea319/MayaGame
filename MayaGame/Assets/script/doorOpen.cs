using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.AI;

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
    NavMeshObstacle obstacle;

    // Use this for initialization
    void Start () {
        mycol = GetComponent<Collider>();
        ui = GameObject.Find("UI-Canvas(Clone)").GetComponent<FPS_UI>();
        if (isServer)
        {

            obstacle = gameObject.AddComponent<NavMeshObstacle>();
            obstacle.center = mycol.bounds.center;
            obstacle.size = mycol.bounds.size;
            obstacle.carving = true;
        }
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
            obstacle.enabled = false;
            RpcOpen();
        }
        else
        {
            timer = openTime;
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
        open = true;
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
            if(openStart && 0 < timer && !open)
            {
                string[] msg = new string[2];
                msg[0] = "Door Hacking...";
                msg[1] = (timer).ToString("f1");
                ui.SetTaskInfo(msg);
            }
        }
        if (isServer)
        {
            if (openStart &&  !open)
            {
                timer -= Time.deltaTime;
                if (0 >= timer)
                {
                    open = true;
                    obstacle.enabled = false;
                    RpcOpen();
                }
            }
        }
	}
}
