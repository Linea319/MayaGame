using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetBag : Bag
{
    public Animator anim;

    public override void Kaisyu(Transform player)
    {
        player.GetComponent<FPSController>().SetBag(true, moveDebuf);
        CmdOpen();
    }

    [Command]
    void CmdOpen()
    {
        RpcOpen();
    }

    [ClientRpc]
    void RpcOpen()
    {
        anim.SetBool("open", true);
        GetComponent<UIMessenger>().enabled = false;
    }
}
