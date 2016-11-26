using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class doorOpen : NetworkBehaviour {
    [SyncVar]
    bool open = false;
    Collider mycol;
    public Animator anim;
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
        open = true;
        RpcOpen();
    }
    [ClientRpc]
    void RpcOpen()
    {
        mycol.enabled = false;
    }

	// Update is called once per frame
	void Update () {
        if (isClient)
        {
            anim.SetBool("open", open);
        }
	}
}
