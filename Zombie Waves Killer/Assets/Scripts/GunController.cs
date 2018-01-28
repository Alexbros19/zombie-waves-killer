using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

	public Transform gunHold;
	public Gun[] startingGun;
    public static bool isShootButtonPressed;
    Gun equippedGun;

	void Awake(){
        //PlayerPrefs.DeleteAll();
        isShootButtonPressed = false;
        if (startingGun != null){
            if (PlayerPrefs.GetInt("pickedweaponnumber") > 0)
            {
                EquipGun(startingGun[PlayerPrefs.GetInt("pickedweaponnumber") - 1]);
            }
            else {
                EquipGun(startingGun[0]);
            }
		}
	}

	public void EquipGun(Gun gunToEquip){
		if(equippedGun != null){
			Destroy (equippedGun.gameObject);
		}
		equippedGun = Instantiate (gunToEquip, gunHold.position, gunHold.rotation) as Gun;
		equippedGun.transform.parent = gunHold;
	}

    public void CheckShootButtonPressed()
    {
        isShootButtonPressed = true;
    }

    public void OnTriggerHold(){
		if(equippedGun != null){
			equippedGun.OnTriggerHold ();
		}
	}

    public void OnTriggerRelease(){
        if (equippedGun != null){
            equippedGun.OnTriggerRelease();
        }
    }
}
