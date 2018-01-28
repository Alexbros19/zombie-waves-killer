using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	public GameObject mainMenuHolder;
	public GameObject optionsMenuHolder;
	public Slider[] volumeSliders;
    [SerializeField]
    private Text highscoreValue;

	void Start(){
		volumeSliders [0].value = AudioManager.instance.musicVolumePercent;
		volumeSliders [1].value = AudioManager.instance.sfxVolumePercent;

        highscoreValue.text = PlayerPrefs.GetInt("ScoreValue").ToString();
	}

	public void Play(){
		SceneManager.LoadScene ("GameWindow");
	}

	public void Exit(){
		Application.Quit ();
	}

	public void OptionsMenu(){
		mainMenuHolder.SetActive (false);
		optionsMenuHolder.SetActive (true);
	}

	public void MainMenu(){
		mainMenuHolder.SetActive (true);
		optionsMenuHolder.SetActive (false);
	}

	public void SetMusicVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
	}

	public void SetSoundsVolume(float value){
		AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Sfx);
	}

    public void ShootgunMenu() {
        SceneManager.LoadScene("ShootgunMenu");
    }
}
