using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ZakoAI : EnemyAI {


    [ServerCallback]
    public override void Update()
    {
        base.Update();
        if(nav.remainingDistance <= nav.stoppingDistance*1.1f)
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
        anim.SetTrigger("Attack");
    }

    public override void AttackStart(int num)
    {
        base.AttackStart(num);
        nav.Stop();
        //Debug.Log("attack_start");
    }

    public override void AttackEnd(int num)
    {
        base.AttackEnd(num);
        nav.Resume();
    }
}
