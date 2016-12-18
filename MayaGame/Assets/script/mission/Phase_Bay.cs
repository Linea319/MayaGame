using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phase_Bay : Phase {

    public UIMessenger message;
    public Animator anim;
    public string animName;
    public float stopTime;
    public float stopRate;
    bool move = false;
    float timer;


    // Update is called once per frame
    public override void StartPhasae()
    {
        base.StartPhasae();
        message.enabled = true;
        timer += stopTime;
    }

    [ServerCallback]
    void Update()
    {
        

        if (move && anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
        {
            if (timer < Time.time)
            {
                if (Random.Range(0, 100) < stopRate)
                {
                    anim.speed = 0;
                    RpcStop();
                }
                timer += stopTime;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
            {
                ClearPhase();
            }
            
        }
    }

    [ClientRpc]
    void RpcStop()
    {
        message.enabled = true;
    }

    public void Anim()
    {
        if (isClient)
        {
            CmdAnim();
            
        }
    }

    [Command]
    public void CmdAnim()
    {
        anim.SetBool("down", true);
        anim.speed = 1;
        move = true;
        //message.enabled = false;
    }

    [ClientRpc]
    void RpcAnim()
    {
        message.enabled = false;
    }
}
