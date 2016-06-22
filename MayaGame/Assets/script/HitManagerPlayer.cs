using UnityEngine;
using System.Collections;

public class HitManagerPlayer :MonoBehaviour{
    public float hitPoint = 100f;
    public float armor = 100f;
    public float rechargeDelay = 3f;
    public float rechargeRate = 50f;
    float rechargeTimer=0f;
    [HideInInspector]public  float maxArmor;
    [HideInInspector]public float maxHP;

	// Use this for initialization
	void Start () {
        Initialize();
	}

    void Initialize()
    {
        maxArmor = armor;
        maxHP = hitPoint;
    }
	
	// Update is called once per frame
	void Update () {
	if(Time.time > rechargeTimer)
        {
            armor += rechargeRate * Time.deltaTime;
            armor = Mathf.Clamp(armor, 0, maxArmor);
        }
	}

    public void HitDamage(float damage)
    {
        Debug.Log("hit:"+damage);
        if (armor <= damage)
        {
            hitPoint -= (damage - armor);
            armor = 0;
            
        }
        else
        {
            armor -= damage;
        }
        rechargeTimer = Time.time + rechargeDelay;
    }
}
