using UnityEngine;
using System.Collections;

public class AttackCall : MonoBehaviour {
    public EffectType type;
    GameObject effect;

    void Start()
    {
        HitEffectManeger hitMng = FindObjectOfType<HitEffectManeger>();
        effect = hitMng.effects[(int)type];
    }

	 void OnTriggerEnter(Collider col)
    {
        //Debug.Log(col.name);
        Instantiate(effect, transform.position, Quaternion.identity);
        if (col.CompareTag("Player"))
        {
            HitPlayer(col.transform);
        }
    }

    /* void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.transform.name);
        // hit.gameObjectで衝突したオブジェクト情報が得られる
        Instantiate(effect, transform.position, Quaternion.identity);
        if (hit.transform.CompareTag("Player"))
        {
            HitPlayer(hit.transform);
        }
    }*/

    public virtual void HitPlayer(Transform target)
    {
        transform.root.GetComponent<EnemyAI>().AttackHit(target.root);
        this.GetComponent<Collider>().enabled = false;
    }
}
