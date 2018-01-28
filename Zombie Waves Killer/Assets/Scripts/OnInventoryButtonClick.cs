using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnInventoryButtonClick : MonoBehaviour, IPointerDownHandler {
    [SerializeField]
    private int buttonNumber;
    [SerializeField]
    private int textZombiesCountToUnlock;
    private static int number;
    private static int zombiesCountToUnlock;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
    }

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

    public static int ZombiesCountToUnlock
    {
        get
        {
            return zombiesCountToUnlock;
        }

        set
        {
            zombiesCountToUnlock = value;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // save click button number
        Number = buttonNumber;
        PlayerPrefs.SetInt("inventorybuttonnumber", Number);
        // save zombies count to unlock
        ZombiesCountToUnlock = textZombiesCountToUnlock;
        PlayerPrefs.SetInt("zombiescounttounlock", ZombiesCountToUnlock);
    }
}
