using UnityEngine;
using System.Collections;

public class UIMessenger : MonoBehaviour {
    public string messagaeText;
    public bool useProgress = true;
    public float progressTime=2f;
    public string compMethhod;
    [HideInInspector] public float progress;
    bool progressNow;

    void Update()
    {
        if (progressNow)
        {
            progress += Time.deltaTime;
            if(progress > progressTime && compMethhod != null)
            {
                gameObject.SendMessage(compMethhod);
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
