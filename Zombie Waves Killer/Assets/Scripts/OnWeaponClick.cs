using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnWeaponClick : MonoBehaviour, IPointerDownHandler {
    [SerializeField]
    private int weaponButtonNumber;
    private static int number;

    public static int Number
    {
        get
        {
            return number;
        }

        set
        {
            number = value;
        }
    }

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Number = weaponButtonNumber;
        // inventory button is pressed and save 
        PlayerPrefs.SetInt("pickedweaponnumber", Number);
    }
}
