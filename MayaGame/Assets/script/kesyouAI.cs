using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class kesyouAI : EnemyAI {
    public GameObject bullet;
    public float bulletSpeed;
    public float spread;
    public Transform gunPos;
    public LayerMask eyeMask;
    bool canRot;


    [ServerCallback]
    public override void Update()
    {
        base.Update();
        if (dead || stopAI )
        {
            anim.SetBool("move", false);
            anim.SetBool("back", false);
            canRot = true;
            return;
        }
        if (nav.desiredVelocity.sqrMagnitude < 1f)
        {
            anim.SetBool("move", false);
            anim.SetBool("back", false);
        }
        else
        {
            if (retreat)
            {
                anim.SetBool("back", true);
              
            }
            else
            {
                anim.SetBool("move", true);
                canRot = false;
            }
            
        }

        if(target != null)
        {
            Vector3 eyePos = transform.position + new Vector3(0,3f,0);
            if (!Physics.Linecast(eyePos, target.position,eyeMask))
            {
                if (atack)
                {
                    attackEmotion += Time.deltaTime * attackEmotionRate.x*1.5f;
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
                    attackEmotion -= Time.deltaTime * attackEmotionRate.y*0.7f;
                }
                else
                {
                    attackEmotion -= Time.deltaTime * attackEmotionRate.y;
                }
            }
            
        }

        if (atack || retreat)
        {
            //anim.SetBool("move", false);
            canRot = true;
            //anim.SetBool("back", false);
        }

        if (canRot)
        {
            Vector3 tarVec = target.position - transform.position;
            tarVec.y = 0f;
            Quaternion newRot = Quaternion.LookRotation(tarVec, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRot, Time.deltaTime * 6);
        }

        //emotion
        attackEmotion = Mathf.Clamp(attackEmotion, 0, 100);
        retreatEmotion = AIAnim.GetFloat("retreate");
        retreatEmotion -= 5 * Time.deltaTime;
        retreatEmotion = Mathf.Clamp(retreatEmotion, 0, 100);

        AIAnim.SetFloat("atacking", attackEmotion);
        AIAnim.SetFloat("retreate", retreatEmotion);
    }

    public override void Dodge()
    {
        int dir = (int)Random.Range(-2,2);
        anim.SetInteger("dodgeDir",dir);
        anim.SetTrigger("dodge");
        distanceEmotion -= 10f;
    }

    public override void Dodge(int dir)
    {
        anim.SetInteger("dodgeDir", dir);
        anim.SetTrigger("dodge");
    }

    public override void Retreat()
    {
        base.Retreat();
        distanceEmotion = 50f;
    }

    public override void Attack()
    {
        
            Shot();
        distanceEmotion += 5f;

    }

    public override void Think()
    {
        if (retreatEmotion > 70f)
        {
            Retreat();
        }
        else
        {
            base.Think();
            distanceEmotion += 10f;
        }
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

    public override void SetHate(Transform target, float hate)
    {
        base.SetHate(target, hate);
        retreatEmotion += hate*0.05f;
        AIAnim.SetFloat("retreate", retreatEmotion);
        //Debug.Log("hate:"+hate);
        
    }
}
