﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Networking;

public class NetAdapter : NetworkBehaviour {
    public Dictionary<string, HitManagerDef> crackObjs = new Dictionary<string, HitManagerDef>();//5.4からユニークID付きで保管
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
        Debug.Log("crack");
    }

    [Server]
    public IEnumerator Death()
    {
        GetComponent<SyncAnim>().CmdSetTrigger("death");
        GetComponent<EnemyAI>().Stop();
        Debug.Log(crackObjs.Count);
        yield return new WaitForSeconds(5f);
        NetworkServer.Destroy(gameObject);
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
        
        if (crackObjs.ContainsKey(objName) && crackObjs[objName] != null)
        {
            crackObjs[objName].SetHP(hp);
        }
    }
}
