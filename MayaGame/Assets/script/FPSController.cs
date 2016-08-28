using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class FPSController : NetworkBehaviour {
	public Animator anim;
    public GameObject UIObj;
    SyncAnim nAnim;
	CharacterController control;
	public float moveSpeed;
	public float runSpeed;
    public float runEnergy;
	public float mouseSensi;
	public GameObject chara;
    public Transform rotPoint;
	[HideInInspector]
	public Vector3 mouseVec;
	public float jumpPower;
    public float speed;
    public float stepDistance;
    public float stepEnergy;
    public float staminaRegene;

	float yVec;
	public float airPower;
	public float airDrag;
    public float moveShake = 0.5f;
    public float shakeRate = 1f;
    float shakeTimer;
    [HideInInspector] public Vector3 moveVec;
    [HideInInspector] public bool run;
    [HideInInspector] public bool crouch;
	float horizonTimer;
	float verticalTimer;
    [HideInInspector]Camera myCamera;
    public Transform camPosition;
    public Transform ADSPosition;
    public Transform handPosR;
    public Transform handPosL;
    [HideInInspector] public bool ADS;
    public float ADSTime = 0.3F;
    [HideInInspector]public float ADSTimer = 0;
    [HideInInspector] public Vector3 shakeVec;
    public float camShakeReturn;
    public WeponInterface useWepon;
    [SyncVar]
    public int useWeponNum = 0;
    [SyncVar]
    Vector3 inputVector;
    public GameObject[] weponPrefab;

    [HideInInspector] public SyncListString weponPath = new SyncListString();
    GameObject[] wepons = new GameObject[2];
    float[] changeSpeed = new float[2];
    bool changeNow;
    [HideInInspector]
    public Vector3 recoilVec;
    [HideInInspector]
    public bool reload;
    Vector3 prePos;
    float CheckTimer = 0;
    [HideInInspector]
    public FPS_UI UICon;
    [HideInInspector]
    public float stamina;
    [HideInInspector]
    public HitManagerPlayer hpMng;
    [HideInInspector]
    public RuntimeAnimatorController defAnim;
    public UIMessenger otherPlayer;
    Transform focusTr;

    //state
    public bool dead;


    // Use this for initialization

    void Start () {
        Debug.Log("start");
        gameObject.name = "Player_" + netId.ToString();
        
        myCamera = Camera.main;
        focusTr = myCamera.transform.GetChild(0);
        if (isLocalPlayer)
        {
            GameObject ui = Instantiate(UIObj);
            UICon = ui.GetComponent<FPS_UI>();
            UICon.FPSCon = this;
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Confined;

        }

    }

    public override void OnStartServer()
    {
        //RpcSetWeponPrefab(weponPrefab[0], weponPrefab[1]);
    }

    public override void OnStartClient()
    {
        
        Debug.Log("client");
        weponPrefab[0] = Resources.Load(weponPath[0]) as GameObject;
        weponPrefab[1] = Resources.Load(weponPath[1]) as GameObject;
        Debug.Log(useWeponNum);
        defAnim = anim.runtimeAnimatorController;
        spawnWepon(1);
        spawnWepon(0);
        Initialize();
    }

    [Client]
    void Initialize()
    {
        transform.position = Vector3.up * 2;
        Debug.Log("player_Initialized");
        hpMng = GetComponent<HitManagerPlayer>();
        //anim = GetComponent<Animator>();
        control = GetComponent<CharacterController>();
        if (wepons[0] != null) { 
            useWepon = wepons[useWeponNum].GetComponent<WeponInterface>();
            wepons[useWeponNum].SetActive(true);
            if (useWeponNum > 0)
            {
                wepons[0].SetActive(false);
            }
            else
            {
                wepons[1].SetActive(false);
            }
        }
        nAnim = this.GetComponent<SyncAnim>();
       
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer)
        {
            if (Time.time > CheckTimer)
            {
                SendAnimMove();
                CheckTimer = Time.time + 0.1f;
            }
            return;
        }
        else {
            inputVector = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
            CmdSetInput( inputVector);
        }
         
            if (Pause.pause) return;
        if (dead)
        {
            deathUpdate();
            return;
        }
		yVec += -9.8f*Time.deltaTime;
		if(control.isGrounded){//ground
			if(!run){
			moveVec = new Vector3(Input.GetAxis("Horizontal")*moveSpeed,yVec,Input.GetAxis("Vertical")*moveSpeed);
				run = false;

			}
			else{
				moveVec = new Vector3(Input.GetAxis("Horizontal")*moveSpeed*0.5f,yVec,Mathf.Clamp(Input.GetAxis("Vertical"),0f,1f)*runSpeed);
                
				run=true;
			}
            if (Input.GetButton("Run"))
            {
                stamina -= (runEnergy + staminaRegene) * Time.deltaTime;
                if (!run)
                {
                    useWepon.CantAttack();
                }
                run = true;
                if (changeNow)
                {
                    changeNow = false;
                }
                if (reload)
                {
                    reload = false;
                }
                if (stamina <= 0)
                {
                    run = false;
                    StartCoroutine(ReturnRun());
                }
            }
            else
            {
                if (run)
                {
                    run = false;
                    StartCoroutine(ReturnRun());
                }
            }
            


            if (ADS)
            {
                moveVec *= 0.5f;
                run = false;
            }
            if(crouch)
            {
                moveVec *= 0.5f;
                run = false;
            }
			//control.Move(transform.rotation*(moveVec*Time.deltaTime));
			if(Input.GetButtonDown("Jump")){
				StartCoroutine(JumpStart());
			}

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = !crouch;
                
            }
           
			//Vector2 inputVec = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
			//inputVec.magnitude - PreInputVec.magnitude
			if(Input.GetButtonDown("Horizontal")){
				if(horizonTimer > Time.time){
					Step();
				}
				else{
				horizonTimer = Time.time+0.2f;
				}
			}
			if(Input.GetButtonDown("Vertical")){
				if(verticalTimer > Time.time){
					Step();
				}
				else{
					verticalTimer = Time.time+0.2f;
				}
			}

            

            anim.SetBool("crouch", crouch);
            anim.SetBool("Run", run);
            //anim.SetBool("Run",run);
            //anim.SetFloat("Xvec",moveVec.x);
            //anim.SetFloat("Yvec",moveVec.z);
            if ( Mathf.Abs(moveVec.x)+Mathf.Abs(moveVec.z) > 0.1f && !ADS)
            {

                if (run)
                {
                    shakeVec += new Vector3(Mathf.Sin(Time.time * shakeRate/1.5f) * moveShake*2f, Mathf.Sin(Time.time * shakeRate*1.5f) * moveShake*2f, 0) * Time.deltaTime;
                }
                else
                {
                    shakeVec += new Vector3(0, Mathf.Sin(Time.time * shakeRate) * moveShake, 0) * Time.deltaTime;
                }
                
            }
            moveVec = transform.rotation*moveVec;
            Vector3 sVec = new Vector3(moveVec.x, 0, moveVec.z);
            speed = sVec.magnitude;
        }
		else{//air
			Vector3 tmpVec = Vector3.Lerp(moveVec,Vector3.zero,airDrag*Time.deltaTime);
			tmpVec = new Vector3(tmpVec.x,yVec,tmpVec.z);
			Vector3 inputVec = new Vector3(Input.GetAxis("Horizontal")*airPower,0,Input.GetAxis("Vertical")*airPower)*Time.deltaTime;
			moveVec = (tmpVec+(transform.rotation*(inputVec)));
            Vector3 sVec = new Vector3(moveVec.x,0,moveVec.z);
            speed = sVec.magnitude;
            crouch = false;
            if (run)
            {
                run = false;
                StartCoroutine(ReturnRun());
            }
            anim.SetBool("crouch", crouch);
            anim.SetBool("Run", run);
            //anim.SetBool("Run",false);

            //anim.SetFloat("Xvec",0);
            //anim.SetFloat("Yvec",0);
        }
        anim.SetBool("ground", control.isGrounded);

        

        //Gun
        if (Input.GetButton("Fire1"))
        {
            useWepon.Primaly();

        }
        else
        {
            useWepon.ReturnPrimaly();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            useWepon.Secondary();

        }
        if (Input.GetButtonDown("Reload"))
        {
            if (!changeNow && !reload && !run)
            {
                useWepon.Reload();
                reload = true;
                nAnim.SetTrigger("Reload");
            }

        }

        if (Input.GetButtonDown("change"))          
        {
            useWepon.CantAttack();
            int num = useWeponNum+1;
            if (num >= wepons.Length)
            {
                num = 0;
            }
            RuntimeAnimatorController nowAnim = anim.runtimeAnimatorController;
            changeSpeed[num] = wepons[num].GetComponent<WeponInterface>().ReturnChangeSpeed(nowAnim.animationClips[5].length);
            anim.SetFloat("changeSpeed", changeSpeed[num]);
            nAnim.SetTrigger("change");
            changeNow = true;
            reload = false;
        }

        if (otherPlayer != null)
        {
           
            if (Input.GetButtonDown("Action"))
            {

                otherPlayer.SetProgress(true);
            }
            if (Input.GetButtonUp("Action"))
            {
                otherPlayer.SetProgress(false);
            }
          bool distOther = (otherPlayer.transform.position - transform.position).sqrMagnitude < 25;
            if (!distOther)
            { 
                otherPlayer.SetProgress(false);
                UICon.SetMessageText(null);
                otherPlayer = null;
            }
           
        }
        else
        {
            UICon.SetMessageText(null);
        }

        //anim.SetBool("air",!control.isGrounded);
        control.Move(moveVec*Time.deltaTime);
        stamina += staminaRegene * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, 100);

		mouseVec += new Vector3(Input.GetAxisRaw("Mouse Y")*-1,Input.GetAxisRaw("Mouse X"),0)*mouseSensi;
		mouseVec = new Vector3(Mathf.Clamp(mouseVec.x,-89,80),mouseVec.y,mouseVec.z);
        //chara.transform.localEulerAngles = new Vector3(mouseVec.x,0,0);
        float xVec = Input.GetAxisRaw("Mouse Y");
        float nowX = chara.transform.localEulerAngles.x + xVec * -1 * mouseSensi*1.1f;
        if (nowX < 271 && nowX > 180)
        {
            xVec = Mathf.Clamp(xVec, -1, 0) + recoilVec.x * Time.deltaTime * 100f;
        }
        if (nowX > 89 && nowX < 180)
        {
            xVec = Mathf.Clamp(xVec, 0, 1);
        }
        chara.transform.RotateAround(chara.transform.position, chara.transform.right, (xVec-recoilVec.x*Time.deltaTime*100f) * -1 *mouseSensi);
        
        transform.eulerAngles = new Vector3(0,mouseVec.y,0);
        shakeVec = Vector3.Lerp(shakeVec, Vector3.zero,camShakeReturn * Time.deltaTime);

	}

    void deathUpdate()
    {
        yVec += -9.8f * Time.deltaTime;
        moveVec = new Vector3(0, yVec, 0);
        control.Move(moveVec * Time.deltaTime);
        stamina += staminaRegene * Time.deltaTime;
        stamina = Mathf.Clamp(stamina, 0, 100);

        mouseVec += new Vector3(Input.GetAxisRaw("Mouse Y") * -1, Input.GetAxisRaw("Mouse X"), 0) * mouseSensi;
        mouseVec = new Vector3(Mathf.Clamp(mouseVec.x, -89, 80), mouseVec.y, mouseVec.z);
        //chara.transform.localEulerAngles = new Vector3(mouseVec.x,0,0);
        float xVec = Input.GetAxisRaw("Mouse Y");
        float nowX = chara.transform.localEulerAngles.x + xVec * -1 * mouseSensi * 1.1f;
        if (nowX < 271 && nowX > 180)
        {
            xVec = Mathf.Clamp(xVec, -1, 0) + recoilVec.x * Time.deltaTime * 100f;
        }
        if (nowX > 89 && nowX < 180)
        {
            xVec = Mathf.Clamp(xVec, 0, 1);
        }
        chara.transform.RotateAround(chara.transform.position, chara.transform.right, (xVec - recoilVec.x * Time.deltaTime * 100f) * -1 * mouseSensi);

        transform.eulerAngles = new Vector3(0, mouseVec.y, 0);
        shakeVec = Vector3.Lerp(shakeVec, Vector3.zero, camShakeReturn * Time.deltaTime);
        useWepon.ReturnPrimaly();
        
        run = false;
        changeNow = false;
        reload = false;
        ADS = false;

        anim.SetBool("crouch", true);
        anim.SetBool("Run", false);
    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Pause.pause) return;
        if (ADS)
        {
            myCamera.transform.position = Vector3.Lerp(camPosition.position, ADSPosition.position, 1 - ADSTimer / ADSTime);
            myCamera.transform.rotation = Quaternion.Lerp(camPosition.rotation, ADSPosition.rotation, 2f*(1 - ADSTimer / ADSTime)-0.5f);
            focusTr.localPosition = new Vector3(0,0, Mathf.Lerp(1, 0.45f, 1 - ADSTimer / ADSTime)); 
        }
        else
        {
            myCamera.transform.position = Vector3.Lerp(ADSPosition.position, camPosition.position, 1 - ADSTimer / ADSTime);
            myCamera.transform.rotation = Quaternion.Lerp(ADSPosition.rotation, camPosition.rotation, 2f * (1 - ADSTimer / ADSTime) + 0.5f);
            focusTr.localPosition = new Vector3(0,0, Mathf.Lerp(0.45f, 1, 1 - ADSTimer / ADSTime));
        }
        ADSTimer -= Time.deltaTime;
        ADSTimer = Mathf.Clamp(ADSTimer, 0, ADSTime);
        myCamera.transform.position += myCamera.transform.rotation * shakeVec;
    }



    IEnumerator JumpStart(){
        nAnim.SetTrigger("jump");
        yield return new WaitForSeconds(0.2f);
        yVec = jumpPower;
    }

	void Step(){
        if (stamina < stepEnergy) return;
		yVec = stepDistance;
		moveVec = new Vector3(Input.GetAxisRaw("Horizontal")*runSpeed,yVec,Input.GetAxisRaw("Vertical")*runSpeed)*1.5f;
        //moveVec = transform.rotation*moveVec;
        stamina -= stepEnergy;
	}

    public void Shake(Vector3 vec)
    {
        shakeVec += vec;
    }

    public void EndChange()
    {
        if (isLocalPlayer)
        {
            useWeponNum++;
            if (useWeponNum >= wepons.Length)
            {
                useWeponNum = 0;
            }
            changeNow = false;
            /*wepons[useWeponNum].SetActive(false);
            
            Debug.Log(useWeponNum);


            useWepon = wepons[useWeponNum].GetComponent<WeponInterface>();
            useWepon.initialize();
            changeNow = false;
            wepons[useWeponNum].SetActive(true);*/
            CmdSetUseNum(useWeponNum);
        }
    }

    IEnumerator ReturnRun()
    {
        yield return new WaitForSeconds(0.5f);
        useWepon.CanAttack();
    }

    [Command]
    void CmdSetUseNum(int num)
    {
        useWeponNum = num;
        RpcChangeWepon( num);
    }

    [ClientRpc]
    void RpcChangeWepon(int num)
    {
        if(num > 0)
        {
            wepons[num - 1].SetActive(false);
        }
        else {
            wepons[wepons.Length-1].SetActive(false);
        }
        wepons[num].SetActive(true);
        useWepon = wepons[num].GetComponent<WeponInterface>();
        useWepon.Setup(num);
    }


    public void spawnWepon(int num)
    {
        Debug.Log("sp");
        GameObject weponObj = Instantiate(weponPrefab[num]);
        weponObj.transform.SetParent(handPosR, false);
        weponObj.transform.localPosition = Vector3.zero;
        weponObj.transform.localEulerAngles = Vector3.zero;
        wepons[num] = weponObj;
        weponObj.GetComponent<WeponInterface>().Setup(num);
        Debug.Log("spawnWepon");
    }

    [Command]
    public void CmdShot()
    {
        RpcShotEffect();
    }

    [ClientRpc]
    public void RpcShotEffect()
    {
        if (!isLocalPlayer)
        {
            useWepon.ShotEffect();
           // Debug.Log("sound");
        }
    }

    [Command]
    void CmdSetInput(Vector3 vec)
    {
        inputVector = vec;
    }

    [Command]
    public void CmdSendHP(string uniqueID,string objName, float HP,float hateNum)
    {
        GameObject target = GameObject.Find(uniqueID);
        NetAdapter targetAdapter = target.GetComponent<NetAdapter>();
        if(targetAdapter != null ){
            targetAdapter.RpcSetHP(objName, HP);
            targetAdapter.SetHate(transform, hateNum);
        }
    }

    [Command]
    public void CmdPlayerDeath()
    {
        RpcPlayerDeath();
    }

    [ClientRpc]
    public void RpcPlayerDeath()
    {
        if (!isLocalPlayer)
        {
            hpMng.Death();
        }
    }

    [ClientRpc]
    public void RpcSetWeponPrefab(GameObject prim, GameObject second)
    {
        Debug.Log("Set Wepon from Lobby");
        weponPrefab[0] = prim;
        weponPrefab[1] = second;
    }


   [Command]
    public void CmdTargetResulect(string uniqueID)
    {
        GameObject target = GameObject.Find(uniqueID);
       target.GetComponent<HitManagerPlayer>().RpcResulect();
    }

    void SendAnimMove()
    {

        anim.SetFloat("moveX", inputVector.x);
        anim.SetFloat("moveY", inputVector.z);

    }

    
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!isLocalPlayer) return;
        UIMessenger messe = hit.transform.GetComponent<UIMessenger>();
        if (messe == null || !messe.enabled) return;
        otherPlayer = messe;
        UICon.SetMessageText(otherPlayer);
    }



    }
