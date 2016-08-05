using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public interface BehaveInterface
{
   void Think();
    void Jump();
}


public class EnemyAI : NetworkBehaviour,BehaveInterface
{
    public float thinkRate = 3f;
    public float effectionRate = 50f;//100=limit
    public LayerMask mask;
    public Animator anim;
    public Animator AIAnim;

    //parameter
    public float moveSpeed = 4f;
    public float defDistance = 25f;

    protected NavMeshAgent nav;
    float thinkTimer;
    public Transform target;
    Vector3 moveTarget;
    bool atack;
    public Collider[] atackCol;
    public float attackDamage = 25f;

    //emotion
    float thinkEmotion=50f;
    float distanceEmotion=50f;

    //state 
    [HideInInspector] public bool dead;
    [HideInInspector] public float shock;
    bool jumpNow;
    float jumpTimer;

    // Use this for initialization
    [ServerCallback]
    void Start () {

        nav = GetComponent<NavMeshAgent>();
        nav.enabled = true;
        nav.speed = moveSpeed;
        //nav.updatePosition = false;
        //nav.updateRotation = false;
	}

    // Update is called once per frame
    [ServerCallback]
    public virtual void Update () {
        if (dead)
        {
            nav.Stop();
            return;
        }

        nav.speed = Mathf.Lerp(moveSpeed, 0, shock / 100f);
        shock = Mathf.Lerp(shock, 0, Time.deltaTime*0.5f);

	    if(Time.time > thinkTimer)
        {
            //Think();
            AIAnim.SetTrigger("start");
            thinkTimer = Time.time + thinkRate;
        }

        if((moveTarget-transform.position).sqrMagnitude > 1f)
        {

        }
        else
        {

        }

        if (nav.isOnOffMeshLink)
        {
            Jump();
        }

	}

    public void Jump()
    {
        OffMeshLinkData data = nav.currentOffMeshLinkData;
        Vector3 endPos = data.endPos;
        Vector3 startPos = data.startPos;
        float height = Mathf.Abs(endPos.y - startPos.y);
        if (!jumpNow)
        {
            transform.LookAt(new Vector3(endPos.x, transform.position.y, endPos.z));
            jumpNow = true;
            jumpTimer = 0;
        }
        jumpTimer += Time.deltaTime;
        if (jumpTimer > 1)
        {
            transform.position = Vector3.Lerp(startPos, endPos, jumpTimer - 1) + new Vector3(0, Mathf.Sin((jumpTimer - 1) * Mathf.PI) * height, 0);
        }
            
        //transform.position.y = Mathf.Sin((jumpTimer - 1) * Mathf.PI);
        Debug.Log(Mathf.Sin((jumpTimer - 1) * Mathf.PI));
        if ((transform.position - endPos).sqrMagnitude < 0.1) 
        {
            nav.CompleteOffMeshLink();
            jumpNow = false;
        }
    }

    public virtual void Think()
    {
        if (target == null) SearchTarget();

        Vector3 vec = (transform.position - target.position);
        vec.y = 0;
        vec = vec.normalized;
        float distance = defDistance * (1f - distanceEmotion / 100f) * 2f;
        Collider[] points = Physics.OverlapSphere(target.position + vec * distance, 10f, mask);
        Debug.DrawLine(target.position + vec * distance, transform.position,Color.blue,1f);
        int posNum = 0;
        if (points.Length > 0 ){
            if(moveTarget == points[posNum].transform.position)
            {
                posNum = Random.Range(0,points.Length);
            }
            moveTarget = points[posNum].transform.position;
            
        }
        else
        {
            moveTarget = (target.position + vec * distance)+Random.insideUnitSphere*5f;
            
        }
        nav.SetDestination(moveTarget);
        thinkTimer = Time.time + thinkRate;
    }

    public virtual void Attack()
    {

    }

    public void SearchTarget()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if(players.Length > 0)
        {
            float distance=float.MaxValue;
            for(int i = 0; i < players.Length; i++)
            {
                float cDis = (transform.position - players[i].transform.position).sqrMagnitude;
                if (distance > cDis)
                {
                    target = players[i].transform;
                    distance = cDis;
                }
            }
        }
    }

    public virtual void AttackStart(int num)
    {
        atackCol[num].enabled = true;         
        atack = true;
    }

    public virtual void AttackEnd(int num)
    {
        atackCol[num].enabled = false;
        atack = false;
    }

    public void AttackHit(Transform target)
    {
        HitManagerPlayer playerHP = target.GetComponent<HitManagerPlayer>();
        playerHP.HitDamage(attackDamage);
        Debug.Log("attackhit");
    }

    public void EnterNear(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            AIAnim.SetTrigger("near");
        }
    }

    public virtual void Stop()
    {
        dead = true;
        nav.Stop();
    }


    public void StopSend(float time)
    {
        StartCoroutine(StopOnTime(time));
    }

     IEnumerator StopOnTime(float time)
    {
        Debug.Log("Yoroke");
        nav.Stop();
        yield return new WaitForSeconds(time);
        Debug.Log("ReturnYoroke");
        nav.Resume();
        //AIAnim.SetTrigger("start");
        //thinkTimer = Time.time + thinkRate;
        
    } 
}
