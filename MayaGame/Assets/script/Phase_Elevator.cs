using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Phase_Elevator : Phase {
    public UIMessenger message;
    public Animator elevatorAnim;
    bool move = false;


    // Update is called once per frame
    public override void StartPhasae()
    {
        base.StartPhasae();
        message.enabled = true;
    }

    [ServerCallback]
    void Update()
    {
        if (move && elevatorAnim.GetCurrentAnimatorStateInfo(0).IsName("elevator")&& elevatorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            ClearPhase();
        }
    }

    [Command]
    public void CmdNext()
    {
        elevatorAnim.SetBool("down", true);
        move = true;
    }
}
