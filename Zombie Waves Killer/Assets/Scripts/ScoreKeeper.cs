using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour {
	public static int score{ get; private set;}

	void Start () {
		ZombieController.OnDeathStatic += OnZombieKilled;
		FindObjectOfType<PlayerController> ().OnDeath += OnPlayerDeath;
		score = 0;
    }
	
	void OnZombieKilled(){
        int randomNumber = Random.Range(40, 60);
        Debug.Log("random number " + randomNumber);

        score += randomNumber;
        Debug.Log("score " + score);

        if (score > PlayerPrefs.GetInt("ScoreValue", 0)) {
            PlayerPrefs.SetInt("ScoreValue", score);
        }
	}

	void OnPlayerDeath(){
		ZombieController.OnDeathStatic -= OnZombieKilled;
	}
}
