using UnityEngine;
using System.Collections;

public class UIMessenger : MonoBehaviour {//メソッド呼び出しはFPS_UIで行う
    public string messagaeText;
    public bool usePlayer = true;
    public bool noAuth = false;
    public bool useTarget = false;
    public float progressTime=2f;
    public string compMethhod;
    public Behaviour methodObj;
    public Texture TargetTex;
    [HideInInspector] public float progress;
    bool progressNow;

    void Start()
    {

    }

    void Update()
    {
        if (progressNow)
        {
            progress += Time.deltaTime;
            if(progress > progressTime && compMethhod != null && compMethhod.Length > 0)
            {
                if(methodObj != null)
                {
                    //methodObj.SendMessage(compMethhod);
                }
                else
                {
                    //gameObject.SendMessage(compMethhod);
                }
                
            }
        }
    }

    void OnGUI()
    {
        if (Vector3.Angle(Camera.main.transform.forward, (transform.position - Camera.main.transform.position)) < Camera.main.fov)
        {
            Vector2 vec = Camera.main.WorldToScreenPoint(transform.position + Vector3.up * 5f);
            vec = new Vector2(vec.x, Screen.height - vec.y);
            Vector2 texVec = new Vector2(TargetTex.width * 0.3f, TargetTex.height * 0.3f);
            Rect nRect = new Rect(vec - texVec * 0.5f, texVec);
            GUI.DrawTexture(nRect, TargetTex);
        }
    }

    public void SetProgress(bool set)
    {
        progressNow = set;
        if (!set)
        {
            progress = 0;
        }
    }
    
}
