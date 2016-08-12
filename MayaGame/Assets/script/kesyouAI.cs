using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class kesyouAI : EnemyAI {
    public GameObject bullet;
    public float bulletSpeed;
    public float spread;
    public Transform gunPos;
    public LayerMask eyeMask;


    [ServerCallback]
    public override void Update()
    {
        base.Update();
        if (dead || stopAI)
        {
            return;
        }
        if (nav.remainingDistance <= nav.stoppingDistance * 1.1f)
        {
            anim.SetBool("move", false);
        }
        else
        {
            anim.SetBool("move", true);
        }

        if(target != null)
        {
            if (!Physics.Linecast(transform.position, target.position,eyeMask))
            {
                if (atack)
                {
                    attackEmotion += Time.deltaTime * attackEmotionRate.x*0.7f;
                }
                else
                {
                    attackEmotion += Time.deltaTime * attackEmotionRate.x;
                } 
            }
            else
            {
                if (atack)
                {
                    attackEmotion -= Time.deltaTime * attackEmotionRate.y*1.5f;
                }
                else
                {
                    attackEmotion -= Time.deltaTime * attackEmotionRate.y;
                }
            }
            attackEmotion = Mathf.Clamp(attackEmotion, 0, 100);
        }

        if (atack)
        {
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            anim.SetBool("move", false);
        }

        AIAnim.SetFloat("atacking", attackEmotion);
    }

    public override void Attack()
    {
        
            Shot();   

    }

    public override void Think()
    {
        base.Think();
        nav.Resume();
    }

    [Server]
    public void Shot()
    {
        Quaternion rot = Quaternion.LookRotation(gunPos.position-target.position);
        Quaternion spreadRot = Quaternion.Euler(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
        //GameObject bulletObj = (GameObject)Network.Instantiate(bullet, gunPos.position, rot,0);
        GameObject bulletObj = (GameObject)Instantiate(bullet, gunPos.position, rot);
        Rigidbody bulletRigid = bulletObj.GetComponent<Rigidbody>();
        bulletRigid.velocity = spreadRot*(target.position - gunPos.position).normalized*bulletSpeed;
        NetworkServer.Spawn(bulletObj);
    }

    public override void AttackStart(int num)
    {
        anim.SetBool("shot", true);
        //base.AttackStart(num);
        if (isServer)
        {
            nav.Stop();
        }
        atack = true;
        //Debug.Log("attack_start");
    }

    public override void AttackEnd(int num)
    {
        anim.SetBool("shot", false);
        //base.AttackEnd(num);
        if (isServer)
        {
            nav.Resume();
        }
        atack = false;
    }
}
