using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
public enum PartsType{
	muzzle,
	handguard,
	upper,
	lower,
	magwell,
	magazine,
	grip,
	stock
}

public interface ParamReciever :  IEventSystemHandler {
	void DivideParm();
    void LoadParam(parameter param);
}

public class parameter : MonoBehaviour,ParamReciever {
	public Dictionary<string,float> parameters = new Dictionary<string,float>(){
		{"damage",0 },
		{"accuracy",0 },
		{"recoil",0},
		{"range",0},
		{"mobility",0},
		{"reload",0},
		{"weight",0}
	};
	public string[] useParams;
	public float weightDef;
	public string[] priolityParams;
	public float priolPercent;
	public float defPoint;
	public int level;
	public string partsName;
	public PartsType partsType;
	public string specialParam;


	// Use this for initialization
	void Start () {
		partsName = gameObject.name;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	virtual public void DivideParm(){
		partsName = gameObject.name.Replace("(Clone)","");
		float tmpParam = useParams.Length*20+ModifiParam();
		priolPercent = Mathf.Clamp(priolPercent,0,50);//divide priority Param
		tmpParam -= priolPercent;
		for(int i=0;i < priolityParams.Length-1;i++){
			//Debug.Log(priolityParams[i])
			parameters[priolityParams[i]] = Random.Range(1,priolPercent);
			priolPercent -= parameters[priolityParams[i]];
			//Debug.Log(parameters[priolityParams[i]]);
		}
		if(priolityParams.Length>0){
			parameters[priolityParams[priolityParams.Length-1]] = priolPercent;
		}
		
		//divide param

		for(int i=0;i < useParams.Length-1;i++){
			float divideBase = tmpParam/(useParams.Length-i);
			float point = Random.Range(divideBase*0.5f,divideBase*1.5f);
			parameters[useParams[i]] += point;
			tmpParam -= point;
			//Debug.Log(parameters[useParams[i]]);
		}
		parameters[useParams[useParams.Length-1]] += tmpParam;
		//Debug.Log(parameters[useParams[useParams.Length-1]]);
		parameters["weight"] = Mathf.Clamp(parameters["weight"],1,100);
		parameters["weight"] = weightDef-weightDef*(parameters["weight"]/100f);
		//Debug.Log(parameters["weight"]);

	}
	virtual public float ModifiParam(){
		return 0;
	}

    virtual public void LoadParam(parameter param)
    {
        parameters = param.parameters;
        
    } 
}
