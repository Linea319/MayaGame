using UnityEngine;
using System.Collections;

public enum EffectType
{
    misc,
    crystal,
}

public class HitEffectManeger : MonoBehaviour {

    public GameObject[] effects;
    
    
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameObject ReturnEffect(EffectType type)
    {
        return effects[(int)type];
    }
}
