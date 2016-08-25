using UnityEngine;
using System.Collections;

public class YebisEnable : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonUp(0))
        {
            YebisPostEffects yebis = GetComponent<YebisPostEffects>();
            if(yebis!=null)
            {
                yebis.enableYebis = !(yebis.enableYebis);
            }
        }
    }
}
