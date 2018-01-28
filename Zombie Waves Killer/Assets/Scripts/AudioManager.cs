using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public enum AudioChannel{Master, Sfx, Music};
	public static AudioManager instance;

	public float masterVolumePercent { get; private set;}
	public float sfxVolumePercent { get; private set;}
	public float musicVolumePercent { get; private set;}
	private int activeMusicSourceIndex;
	private Transform audioListener;
	private Transform playerTransform;
	AudioSource[] musicSources;
	AudioSource sfx2DSource;
	SoundsLibrary library;

	void Awake () {

		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;

			musicSources = new AudioSource[2];
			for(int i=0; i<2; i++){
				GameObject newMusicSource = new GameObject ("Music source " + (i + 1));
				musicSources[i] = newMusicSource.AddComponent<AudioSource> ();
				newMusicSource.transform.parent = transform;
			}

			GameObject newSfx2DSource = new GameObject ("2D sfx source");
			sfx2DSource = newSfx2DSource.AddComponent<AudioSource> ();
			newSfx2DSource.transform.parent = transform;

			audioListener = FindObjectOfType<AudioListener> ().transform;
			if(FindObjectOfType<PlayerController> () != null){
				playerTransform = FindObjectOfType<PlayerController> ().transform;
			}
			library = FindObjectOfType<SoundsLibrary> ();

			masterVolumePercent = PlayerPrefs.GetFloat ("master volume", 1f);
			sfxVolumePercent = PlayerPrefs.GetFloat ("sfx volume", 1f);
			musicVolumePercent = PlayerPrefs.GetFloat ("music volume", 1f);
		}
	}

	void Update(){
		if(playerTransform != null){
			audioListener.position = playerTransform.position;
		}
	}

	public void SetVolume(float volumePercent, AudioChannel channel){
		switch(channel){
		case AudioChannel.Master:
			masterVolumePercent = volumePercent;
			break;
		case AudioChannel.Sfx:
			sfxVolumePercent = volumePercent;
			break;
		case AudioChannel.Music:
			musicVolumePercent = volumePercent;
			break;
		}

		musicSources [0].volume = musicVolumePercent * masterVolumePercent;
		musicSources [1].volume = musicVolumePercent * masterVolumePercent;

		PlayerPrefs.SetFloat ("master volume", masterVolumePercent);
		PlayerPrefs.SetFloat ("sfx volume", sfxVolumePercent);
		PlayerPrefs.SetFloat ("music volume", musicVolumePercent);
		PlayerPrefs.Save ();
	}

	public void PlaySound(AudioClip clip, Vector3 position){
		if(clip != null){
			AudioSource.PlayClipAtPoint (clip, position, sfxVolumePercent * masterVolumePercent); 
		}
	}

	public void PlaySound(string soundName, Vector3 position){
		PlaySound (library.GetClipFromName(soundName), position);
	}

	public void PlaySound2D(string soundName){
		sfx2DSource.PlayOneShot (library.GetClipFromName(soundName), sfxVolumePercent * masterVolumePercent);
	}

	public void PlayMusic(AudioClip clip, float fadeDuration = 1){
		activeMusicSourceIndex = 1 - activeMusicSourceIndex;
		musicSources [activeMusicSourceIndex].clip = clip;
		musicSources [activeMusicSourceIndex].Play ();

		StartCoroutine (AnimateMusicCrossfade(fadeDuration));
	}

	IEnumerator AnimateMusicCrossfade(float duration){
		float percent = 0;

		while(percent < 1){
			percent += Time.deltaTime * 1 / duration;
			musicSources [activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolumePercent * masterVolumePercent, percent);
			musicSources [1 - activeMusicSourceIndex].volume = Mathf.Lerp (musicVolumePercent * masterVolumePercent, 0, percent);
			yield return null;
		}
	}
}
