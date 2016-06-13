using UnityEngine;
using System.Collections;

public class AnimSendTrigger : MonoBehaviour {
    public Animator anim;


    public void SendTrigger(string name)
    {
        anim.SetTrigger(name);
    }

    public void ReloadOpen()
    {
        anim.SetTrigger("open");
    }

    public void ReloadClose()
    {
        anim.SetTrigger("close");
    }

}
