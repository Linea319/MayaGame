using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems; 

public class WeponController : MonoBehaviour {
	Camera myCamera;
	public Animator anim;
	public Transform camPosition;
	public Transform ADSPosition;
	bool ADS;
	public float ADSTime = 0.3F;
	float ADSTimer=0;
	public Transform depthTarget;
	float rpmTimer=0;
	public FPSController FPSCon;
	Vector3 recoilDmp;
	float recoilOffset;
	public Inventory inventry;
	int[] slotNumbers = new int[8];
	bool prePause;

	 public gun myGun;
	// Use this for initialization
	void Start () {
		myCamera = Camera.main;
		//LoadSlot(0);
		//camPosition.rotation = Quaternion.Euler(new Vector3(0,-ADSPosition.eulerAngles.y,0));
	}

	void Update(){
		if(prePause == true && Pause.pause == false) {
            //LoadSlot(0);
        }
		prePause = Pause.pause;

		if(Pause.pause) {
			return;
		}

		RaycastHit hit;
		Physics.Raycast(myCamera.transform.position,myCamera.transform.forward,out hit, 100f);
		depthTarget.localPosition = new Vector3(0,0,hit.distance);
		if(Input.GetButton("Fire1")){
			Shoot();
		}
		if(Input.GetButtonUp("Fire1")){
			StopShoot();
		}
		if(Input.GetButtonDown("Fire2")){
			ADS = !ADS;
			ADSTimer=ADSTime;
		}
		anim.SetBool("ADS",ADS);
		ADSTimer -= Time.deltaTime;
		ADSTimer = Mathf.Clamp(ADSTimer,0,ADSTime);
		FPSCon.mouseVec += recoilDmp;
		recoilDmp = Vector3.Slerp(recoilDmp,Vector3.zero,5*Time.deltaTime);
		recoilOffset = Mathf.Lerp(recoilOffset,0,5*Time.deltaTime);


	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(Pause.pause) return;
			if(ADS){
				myCamera.transform.position =  Vector3.Lerp(camPosition.position, ADSPosition.position,1-ADSTimer/ADSTime);
				myCamera.transform.rotation = Quaternion.Lerp(camPosition.rotation,ADSPosition.rotation,1-ADSTimer/ADSTime);
			}
			else{
				myCamera.transform.position =  Vector3.Lerp(ADSPosition.position, camPosition.position,1-ADSTimer/ADSTime);
				myCamera.transform.rotation = Quaternion.Lerp(ADSPosition.rotation,camPosition.rotation,1-ADSTimer/ADSTime);
			}

		myCamera.transform.position += myCamera.transform.rotation*new Vector3(0,0,recoilOffset);
	}

	void Shoot(){
		if (rpmTimer > Time.time){
			return;
		}
		float accuracy = 2-(Mathf.Sqrt( myGun.parameters["accuracy"])*0.1f);
		accuracy = Mathf.Clamp(accuracy,0.1f,2f);
		Vector3 randomCone = Quaternion.Euler(new Vector3(Random.Range(-accuracy,accuracy),Random.Range(-accuracy,accuracy),0))*myCamera.transform.forward;
		RaycastHit hit;
        Debug.Log("shot");
        if (Physics.Raycast(myCamera.transform.position,randomCone,out hit, myGun.parameters["range"])){
			GameObject efect = Instantiate<GameObject>(myGun.magazineP.hitEffect);
			efect.transform.position = hit.point;
			efect.transform.LookAt(efect.transform.position+ hit.normal);
            Debug.Log(hit.transform.name);
            hit.transform.SendMessage("OnBulletHit", hit.point,SendMessageOptions.DontRequireReceiver);
        }
		myGun.muzuleP.muzuleFlash.emissionRate = myGun.upperP.rpm/60;
		myGun.muzuleP.muzuleFlash.Play ();
		myGun.muzuleP.shotsound.Play();
//		Debug.Log(myGun.parameters["recoil"]/100);
		float recoil = (2-myGun.parameters["recoil"]/100f);
		recoilDmp = new Vector3(-recoil*0.2f,recoil*Mathf.Sign(Random.Range(-1,1))*0.1f,0);//recoil
		recoilOffset = recoil*0.05f; 
		rpmTimer = Time.time + (60f/myGun.upperP.rpm);


	}

	void StopShoot(){
		myGun.muzuleP.muzuleFlash.Stop ();
	}

	public void LoadSlot(int slotNum){
		slotNumbers = new int[8];
		if(inventry.LoadGunParts()){
			slotNumbers = inventry.gunslot[slotNum].partsNumber;
		}

		StartCoroutine("CreateGun");
	}

	public IEnumerator CreateGun(){
		Dictionary<string,GameObject> gunParts = new Dictionary<string,GameObject>();
		foreach ( Transform n in myGun.transform )
		{
			GameObject.Destroy(n.gameObject);
		}
		yield return new WaitForEndOfFrame();
		bool first = inventry.LoadSlot();
		Debug.Log(first);                                 
		if(!first){
			gunParts["muzzle"] = Instantiate(Resources.Load("parts/"+"muzzle"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["muzzle"], null, (x,y)=>x.DivideParm());
			gunParts["handguard"] =Instantiate(Resources.Load("parts/"+"handGuard"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["handguard"], null, (x,y)=>x.DivideParm());
			gunParts["upper"] =Instantiate(Resources.Load("parts/"+"upReciever"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["upper"], null, (x,y)=>x.DivideParm());
			gunParts["lower"] =Instantiate(Resources.Load("parts/"+"lower"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["lower"], null, (x,y)=>x.DivideParm());
			gunParts["magwell"] =Instantiate(Resources.Load("parts/"+"magwell"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["magwell"], null, (x,y)=>x.DivideParm());
			gunParts["magazine"] =Instantiate(Resources.Load("parts/"+"mag"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["magazine"], null, (x,y)=>x.DivideParm());
			gunParts["grip"] =Instantiate(Resources.Load("parts/"+"grip"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["grip"], null, (x,y)=>x.DivideParm());
			gunParts["stock"] =Instantiate(Resources.Load("parts/"+"stock"))as GameObject;
			ExecuteEvents.Execute<ParamReciever>(gunParts["stock"], null, (x,y)=>x.DivideParm());
		}
		else{
			gunParts["muzzle"] = Instantiate(Resources.Load("parts/"+inventry.muzuleSlot[slotNumbers[0]].partsName))as GameObject;
			gunParts["handguard"] =Instantiate(Resources.Load("parts/"+inventry.handGuardSlot[slotNumbers[1]].partsName))as GameObject;
			gunParts["upper"] =Instantiate(Resources.Load("parts/"+inventry.upperSlot[slotNumbers[2]].partsName))as GameObject;
			gunParts["lower"] =Instantiate(Resources.Load("parts/"+inventry.lowerSlot[slotNumbers[3]].partsName))as GameObject;
			gunParts["magwell"] =Instantiate(Resources.Load("parts/"+inventry.magwellSlot[slotNumbers[4]].partsName))as GameObject;
			gunParts["magazine"] =Instantiate(Resources.Load("parts/"+inventry.magazineSlot[slotNumbers[5]].partsName))as GameObject;
			gunParts["grip"] =Instantiate(Resources.Load("parts/"+inventry.gripSlot[slotNumbers[6]].partsName))as GameObject;
			gunParts["stock"] =Instantiate(Resources.Load("parts/"+inventry.stockSlot[slotNumbers[7]].partsName))as GameObject;
		}
		Transform guntr = myGun.GetComponent<Transform>();
		gunParts["muzzle"].transform.SetParent(guntr,false);
		gunParts["handguard"].transform.SetParent(guntr,false);
		gunParts["upper"].transform.SetParent(guntr,false);
		gunParts["lower"].transform.SetParent(guntr,false);
		gunParts["magwell"].transform.SetParent(guntr,false);
		gunParts["magazine"].transform.SetParent(guntr,false);
		gunParts["grip"].transform.SetParent(guntr,false);
		gunParts["stock"].transform.SetParent(guntr,false);
		gunParts["lower"].transform.localPosition = Vector3.zero;
		gunParts["lower"].transform.localEulerAngles = Vector3.zero;
		gunParts["upper"].transform.position = gunParts["lower"].transform.FindChild("upperSlot").position;
		gunParts["stock"].transform.position = gunParts["lower"].transform.FindChild("stockSlot").position;
		gunParts["grip"].transform.position = gunParts["lower"].transform.FindChild("gripSlot").position;
		gunParts["magwell"].transform.position = gunParts["lower"].transform.FindChild("magwellSlot").position;
		gunParts["handguard"].transform.position = gunParts["upper"].transform.FindChild("handguardSlot").position;
		gunParts["muzzle"].transform.position = gunParts["handguard"].transform.FindChild("muzzleSlot").position;
		Debug.Log("set");
		if (!first)
		{
			inventry.addParts(gunParts["muzzle"].GetComponent<parameter>());
			inventry.addParts(gunParts["handguard"].GetComponent<parameter>());
			inventry.addParts(gunParts["upper"].GetComponent<parameter>());
			inventry.addParts(gunParts["lower"].GetComponent<parameter>());
			inventry.addParts(gunParts["magwell"].GetComponent<parameter>());
			inventry.addParts(gunParts["magazine"].GetComponent<parameter>());
			inventry.addParts(gunParts["grip"].GetComponent<parameter>());
			inventry.addParts(gunParts["stock"].GetComponent<parameter>());
			inventry.SaveSlot();
			SaveSlot(0);
			SaveSlot(1);
			SaveSlot(2);
		}
		else
		{
			ExecuteEvents.Execute<ParamReciever>(gunParts["muzzle"], null, (x, y) => x.LoadParam(inventry.muzuleSlot[slotNumbers[0]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["handguard"], null, (x, y) => x.LoadParam(inventry.handGuardSlot[slotNumbers[1]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["upper"], null, (x, y) => x.LoadParam(inventry.upperSlot[slotNumbers[2]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["lower"], null, (x, y) => x.LoadParam(inventry.lowerSlot[slotNumbers[3]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["magwell"], null, (x, y) => x.LoadParam(inventry.magwellSlot[slotNumbers[4]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["magazine"], null, (x, y) => x.LoadParam(inventry.magazineSlot[slotNumbers[5]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["grip"], null, (x, y) => x.LoadParam(inventry.gripSlot[slotNumbers[6]]));
			ExecuteEvents.Execute<ParamReciever>(gunParts["stock"], null, (x, y) => x.LoadParam(inventry.stockSlot[slotNumbers[7]]));
		}
		
		myGun.CombineParam();
		
	}

	public void SaveSlot(int slotNum){
		inventry.gunslot[slotNum].partsNumber = slotNumbers; 
		inventry.SaveGunParts();
	}

}
