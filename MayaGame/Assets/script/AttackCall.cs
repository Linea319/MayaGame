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
            transform.GetComponent<EnemyAI>().AttackHit(col);
        }
    }
}
