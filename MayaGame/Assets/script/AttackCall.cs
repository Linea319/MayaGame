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
            transform.root.GetComponent<EnemyAI>().AttackHit(col.transform.root);
            this.GetComponent<Collider>().enabled = false;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.transform.name);
        // hit.gameObjectで衝突したオブジェクト情報が得られる
        if (hit.transform.CompareTag("Player"))
        {
            transform.root.GetComponent<EnemyAI>().AttackHit(hit.transform.root);
        }
    }
}
