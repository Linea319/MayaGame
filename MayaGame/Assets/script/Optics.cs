﻿using UnityEngine;
using System.Collections;

public class Optics : Atachment {
    public Transform ADSPosition;
    public float ADSFov = 1f;

    public override void initialize(Wepon wep)
    {
        wep.ADSPosition = ADSPosition;
        wep.ADSRate = ADSFov;
        base.initialize(wep);
        Destroy(wep.opticTr.GetChild(0).gameObject);
        transform.parent = wep.opticTr;
        transform.localPosition = offset;
        Debug.Log(transform.localPosition);
    }
}
