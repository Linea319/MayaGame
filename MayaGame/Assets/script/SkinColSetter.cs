using UnityEngine;
using System.Collections;

public class SkinColSetter : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        SkinnedMeshRenderer mesh = GetComponent<SkinnedMeshRenderer>();
        GameObject obj = new GameObject(name + "_col");
        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;
        MeshCollider col = obj.AddComponent<MeshCollider>();
        col.sharedMesh = mesh.sharedMesh;
        col.convex = true;
        obj.transform.parent = mesh.rootBone;
        
        HitManeger hit = obj.AddComponent<HitManeger>();
        HitManeger hitOri = GetComponent<HitManeger>();
        hit.mesh = mesh;
        hit.meshBounds = mesh.bounds;
        hit.armor = hitOri.armor;
        hit.hitPoint = hitOri.hitPoint;
        hit.effectType = hitOri.effectType;
        hit.yoroke = hitOri.yoroke;
        hit.yorokePatern = hitOri.yorokePatern;
        hit.damageDebufRate = hitOri.damageDebufRate;
        hit.speedDebufRate = hitOri.speedDebufRate;
        hit.Initialize();
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
