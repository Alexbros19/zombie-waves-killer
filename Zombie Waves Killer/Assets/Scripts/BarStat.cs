using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class BarStat {
    [SerializeField]
    private Healthbar bar;
    [SerializeField]
    private float maxValue;
    [SerializeField]
    private float currentValue;
    private const int HEALTHMAXVALUE = 500; 

    public float CurrentValue
    {
        get { return currentValue; }
        set {
            currentValue = Mathf.Clamp(value, 0, HEALTHMAXVALUE);
            bar.Value = currentValue;
        }
    }

    public float MaxValue
    {
        get
        {
            return maxValue;
        }

        set
        {
            maxValue = value;
            bar.MaxValue = maxValue;
        }
    }

    public void Initialize() {
        this.MaxValue = maxValue;
        this.CurrentValue = currentValue;
    }
}
