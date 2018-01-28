using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour {
	public AudioClip[] mainTheme;
	public AudioClip menuTheme;
	string sceneName;

	void Start () {
		OnLevelWasLoaded (0);
	}
	
	void OnLevelWasLoaded(int sceneIndex){
		string newSceneName = SceneManager.GetActiveScene ().name;
		if(newSceneName != sceneName){
			sceneName = newSceneName;

            if (sceneName == "Menu")
            {
                Invoke("PlayMusic", .2f);
            }
            else if (sceneName == "GameWindow") {
                Invoke("PlayMusic", 5f);
            }
		}
	}

	void PlayMusic(){
		AudioClip clipToPlay = null;
        if (sceneName == "Menu") {
            clipToPlay = menuTheme;
        } else if (sceneName ==  "GameWindow") {
            clipToPlay = mainTheme[Random.Range(0, 2)];
        }

		if(clipToPlay != null){
			AudioManager.instance.PlayMusic (clipToPlay, 2);
			Invoke ("PlayMusic", clipToPlay.length);
		}
	}
}
