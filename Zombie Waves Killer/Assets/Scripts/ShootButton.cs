using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static bool isShootButtonPressed;

    private void Start()
    {
        isShootButtonPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isShootButtonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isShootButtonPressed = false;
    }
}
