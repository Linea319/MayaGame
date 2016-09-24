using UnityEngine;
using System.Collections;

public class UIMessenger : MonoBehaviour {
    public string messagaeText;
    public bool usePlayer = true;
    public bool noAuth = false;
    public float progressTime=2f;
    public string compMethhod;
    public Behaviour methodObj;
    [HideInInspector] public float progress;
    bool progressNow;

    void Update()
    {
        if (progressNow)
        {
            progress += Time.deltaTime;
            if(progress > progressTime && compMethhod != null && compMethhod.Length > 0)
            {
                if(methodObj != null)
                {
                    methodObj.SendMessage(compMethhod);
                }
                else
                {
                    gameObject.SendMessage(compMethhod);
                }
                
            }
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
