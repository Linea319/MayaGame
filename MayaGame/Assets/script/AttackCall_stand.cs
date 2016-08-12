using UnityEngine;
using System.Collections;

public class AttackCall_stand : AttackCall {
    public float damage;
    public override void HitPlayer(Transform target)
    {
        HitManagerPlayer playerHP = target.root.GetComponent<HitManagerPlayer>();
        playerHP.HitDamage(damage);
    }
    
}
