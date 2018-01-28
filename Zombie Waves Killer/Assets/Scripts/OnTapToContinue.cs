using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnTapToContinue : MonoBehaviour, IPointerClickHandler {

    private static bool isOnTapToContinue;

    public static bool IsOnTapToContinue
    {
        get
        {
            return isOnTapToContinue;
        }

        set
        {
            isOnTapToContinue = value;
        }
    }

    private void Start()
    {
        IsOnTapToContinue = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ReviveButton.isReviveButtonPressed = false;
        IsOnTapToContinue = true;
    }
}
