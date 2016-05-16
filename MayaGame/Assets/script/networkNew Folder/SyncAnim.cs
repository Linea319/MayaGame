using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class SyncAnim : NetworkBehaviour
{
    public Animator anim;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Client]
    public void SetTrigger(string paramName)
    {
        anim.SetTrigger(paramName);
        CmdSetTrigger(paramName);
    }

    [Command]
    public void CmdSetTrigger(string paramName)
    {
        RpcSetTrigger(paramName);
    }

    [ClientRpc]
    public void RpcSetTrigger(string paramName)
    {
        anim.SetTrigger(paramName);
    }

    
}
