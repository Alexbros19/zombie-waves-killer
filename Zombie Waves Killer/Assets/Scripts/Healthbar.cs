using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour {
    private float fillAmountValue;
    private float valueLerpSpeed = 2f;
    [SerializeField]
    private Image content;
    [SerializeField]
    private Text valueText;
    [SerializeField]
    private Color fullColor;
    [SerializeField]
    private Color lowColor;

    public float MaxValue { get; set; }

    public float Value {
        set
        {
            string[] temp = valueText.text.Split(':');
            valueText.text = temp[0] + ": " + value;
            fillAmountValue = CalculateFillAmount(value, 0, MaxValue, 0, 1);
        }
    }

    void Start () {
		
	}
	
	void Update () {
        HandleBar();
	}

    private void HandleBar() {
        if (fillAmountValue != content.fillAmount) {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmountValue, valueLerpSpeed * Time.deltaTime);
        }

        content.color = Color.Lerp(lowColor, fullColor, fillAmountValue);
    }

    private float CalculateFillAmount(float value, float inMin, float inMax, float outMin, float outMax) {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
