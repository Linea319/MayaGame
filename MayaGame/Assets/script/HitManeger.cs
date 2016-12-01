using UnityEngine;
using Exploder;


[System.Serializable]
public class ArmorParameter : System.Object
{

    public float shockResist = 1f;
    public float armorResist = 1f;
    public float heatResist = 1f;

}

public class HitManeger : HitManagerDef {
    ExploderObject exploder;
    public int yorokePatern;
    public float yorokeTime = 2.5f;

    // Use this for initialization
    void Start () {

        Initialize();
    }

    public override void Initialize()
    {
        exploder = FindObjectOfType<ExploderObject>();
             
        if (col == null)
        {
            col = GetComponent<Collider>();
            if (col == null) return;
        }
        if(mesh == null)
        {
            mesh = GetComponent<Renderer>();
        }

        colSize = col.bounds.size.magnitude;
        meshBounds = col.bounds;
        sizeMagnitude = meshBounds.extents.magnitude;
        maxHitPoint = hitPoint;

        net = transform.root.GetComponent<NetAdapter>();
        
        if (net.isClient && GetComponent<SkinColSetter>() == null)
        {
            //Debug.Log(this.name);
            net.crackObjs.Add(gameObject.name, this);
        }

        
    }

    // Update is called once per frame
    void Update () {
	    if(hitPoint <= 0)
        {
            Destruct();
            Destroy(gameObject);
        }
       // Debug.Log(meshBounds.center);
	}

    public override void Destruct()
    {
        if (yoroke)
        {
            EnemyAI ai = transform.root.GetComponent<EnemyAI>();
            ai.Debuf(speedDebufRate,damageDebufRate);
            if (ai.dead) return;
            
            ai.anim.SetInteger("hitPattern", yorokePatern);
            ai.syncAnim.SetTrigger("hit");         
            ai.StopSend(yorokeTime);
        }

        if(mesh != null)
        {
            Debug.Log("destruct");
            exploder.ExplodeObject(mesh.gameObject);
            
            return;
        }
        exploder.ExplodeObject(gameObject);
    }

    public override Vector3 HitDamage(DamageParameter damages,RaycastHit hitInfo,Ray ray)
    {
        
        float pointRate = (1f - Vector3.Distance(mesh.bounds.center, hitInfo.point) / sizeMagnitude)*1.5f;
        Debug.DrawLine(hitInfo.point, ray.origin,Color.red,1f);
        pointRate = Mathf.Clamp01(pointRate);
        //Debug.Log(pointRate);
        float damage = damages.shock / armor.shockResist*pointRate;
        Ray returnRay = new Ray(ray.GetPoint((hitInfo.distance+colSize) * 2f), -ray.direction);
        RaycastHit returnHit;
        hitInfo.collider.Raycast(returnRay, out returnHit, (hitInfo.distance+colSize)*4f);
        Debug.DrawLine(returnHit.point, returnRay.origin, Color.yellow, 1f);
        float penetrateLength = Vector3.Distance(hitInfo.point,returnHit.point);
        float penetrateNum = damages.penetration*0.001f - penetrateLength * armor.armorResist;
        //Debug.Log("armorLength:"+penetrateLength+",penetrate:"+penetrateNum);
        Vector3 rePoint = Vector3.zero;
        if(penetrateNum >0)
        {
            //pointRate = (1f - rayDistance(ray, hitInfo.point) / sizeMagnitude);
            damage += penetrateNum*1000f * pointRate * pointRate;
            
            rePoint = returnHit.point;
        }
        hitPoint -= damage;
        lastDamage = damage;
        //Debug.Log("col:" + name + " damage:" + damage + " penetrate:"+ penetrateNum);

        EnemyAI ai = transform.root.GetComponent<EnemyAI>();
        ai.shock += damages.shock / armor.shockResist * 0.06f;

        return rePoint;
    }
}
