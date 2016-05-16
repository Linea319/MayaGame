using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NetAdapter : NetworkBehaviour {
    public Dictionary<string, HitManagerDef> crackObjs = new Dictionary<string, HitManagerDef>();
	// Use this for initialization
	void Start () {
        gameObject.name += netId.ToString();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    [Command]
    public void CmdCrack(string objName)
    {
        RpcCrack(objName);
    }

    [ClientRpc]
    public void RpcCrack(string objName)
    {
        if(crackObjs[objName] != null)
        {
            crackObjs[objName].Destruct();
        }
    }

    [ClientRpc]
    public void RpcSetHP(string objName,float hp)
    {
        //Debug.Log(objName+":"+hp.ToString());
        if (crackObjs[objName] != null)
        {
            crackObjs[objName].SetHP(hp);
        }
    }
}
