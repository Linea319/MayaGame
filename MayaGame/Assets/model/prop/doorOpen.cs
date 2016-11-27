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
    float timer = 0;
    public GameObject openObject;
	// Use this for initialization
	void Start () {
        mycol = GetComponent<Collider>();
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
    }

    [ClientRpc]
    void RpcOpenStart()
    {
        if(openObject != null)
        {
            openObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
        if (isClient)
        {
            anim.SetBool("open", open);
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
