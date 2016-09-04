using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZakoAI : EnemyAI {


    [ServerCallback]
    public override void Update()
    {
        base.Update();
        if (dead || stopAI)
        {
            return;
        }
        if (nav.remainingDistance <= nav.stoppingDistance*1.1f)
        {
            anim.SetBool("move", false);
        }
        else
        {
            anim.SetBool("move", true);
        }
    }

    public override void Attack()
    {
        syncAnim.SetTrigger("Attack");
    }

    public override void AttackStart(int num)
    {
        
        base.AttackStart(num);
        if (isServer)
        {
            anim.SetBool("move", false);
            nav.Stop();
        }
        
        //Debug.Log("attack_start");
    }

    public override void AttackEnd(int num)
    {
        
        base.AttackEnd(num);
        if (isServer)
        {
            nav.Resume();
        }
    }
}
