using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {
	public Image fadePlane;
	public GameObject gameOverUI;
	public GameObject buttonsControlUI;
	public GameObject playerHealthBar;
	public GameObject zombieCounterUI;
	public RectTransform newWaveBanner;
	public Text newWaveTitle;
	public Text newWaveZombieCount;
	public Text gameOverScoreUI;
	public Text scoreUI;
	public Text zombieAliveCountUI;
    public Text tapToCountinue;
    private ZombieSpawner spawner;
	private float fadePlaneTimer = 1f;
    private bool isCoroutine = true;
    private bool isGameOver;
    private bool isReviveButtonPressed;
    private bool isInfiniteWave;
    private static bool isPlayerDead = false;
    [SerializeField]
    private Image bloodImage;
    [SerializeField]
    private GameObject reviveUI;
    [SerializeField]
    private GameObject playerView;
    private GameObject gun;

    public static bool IsPlayerDead
    {
        get
        {
            return isPlayerDead;
        }

        set
        {
            isPlayerDead = value;
        }
    }

    void Awake(){
		spawner = FindObjectOfType<ZombieSpawner> ();
		spawner.OnNewWave += OnNewWave;
    }

	void Start () {
        if (Application.internetReachability != NetworkReachability.NotReachable && !ReviveButton.isReviveButtonPressed){
            FindObjectOfType<PlayerController>().OnPlayerDeath += OnReviveUI;
        }
        else { // if death and no internet connection
            FindObjectOfType<PlayerController>().OnPlayerDeath += OnGameOver;
        }
        //FindObjectOfType<PlayerController> ().OnDeath += OnGameOver;
        gun = FindObjectOfType<Gun>().gameObject;
        isGameOver = true;
        isReviveButtonPressed = false;
        isInfiniteWave = false;
        isPlayerDead = false;
    }

    void Update(){
		scoreUI.text = ScoreKeeper.score.ToString ("D5");
        if (!isInfiniteWave) {
            zombieAliveCountUI.text = ZombieSpawner.zombiesRemainingAlive.ToString();
        }

        if (ZombieController.IsZombieAttack) {
            StartCoroutine(FadeBloodyScreen(Color.clear, new Color(1, 1, 1, .4f), 1f));
            ZombieController.IsZombieAttack = false;
        }
        // when player health bar is 0
        if (PlayerController.IsPlayerDead)
        {
            playerView.SetActive(false);
            gun.SetActive(false);
            buttonsControlUI.SetActive(false);
        }
        else{
            playerView.SetActive(true);
            gun.SetActive(true);
            buttonsControlUI.SetActive(true);
        }
        // when press on tap to countinue
        if (OnTapToContinue.IsOnTapToContinue && isGameOver) {
            OnGameOver();
            isGameOver = false;
            isPlayerDead = false;
        }
        // when press revive button
        if (ReviveButton.isReviveButtonPressed && !isReviveButtonPressed) {
            reviveUI.SetActive(false);
            StartCoroutine(Fade(new Color(0, 0, 0, .75f), Color.clear,  fadePlaneTimer));
            isReviveButtonPressed = true;
            isPlayerDead = false;
            FindObjectOfType<PlayerController>().OnPlayerDeath += OnGameOver;
        }
    }

    void OnNewWave(int waveNumber){
		string[] numbers = { "One", "Two", "Three", "Four", "Five" };
		newWaveTitle.text = "- Wave " + numbers [waveNumber - 1] + " -";
		string zombieCountString = ((spawner.waves [waveNumber - 1].infinite) ? "Infinite" : spawner.waves [waveNumber - 1].zombieCount + "");
		newWaveZombieCount.text = "Zombies: " + zombieCountString;

        if (spawner.waves[waveNumber - 1].infinite) {
            zombieAliveCountUI.text = "In";
            isInfiniteWave = true;
        }

		StartCoroutine (AnimateNewWaveBanner());
	}

    public void OnReviveUI() {
        // stop spawn zombies
        isPlayerDead = true;

        StartCoroutine(Fade(Color.clear, new Color(0, 0, 0, .75f), fadePlaneTimer));
        StartCoroutine(FadeText(Color.white, new Color(1, 1, 1, 0.3f), 0.75f));
        buttonsControlUI.SetActive(false);
        reviveUI.SetActive(true);
    }

	public void OnGameOver(){
		StartCoroutine (Fade(Color.clear, new Color(0, 0, 0, .75f), fadePlaneTimer));
		gameOverScoreUI.text = scoreUI.text;
		scoreUI.gameObject.SetActive (false);
		gameOverUI.SetActive (true);
		playerHealthBar.SetActive (false);
		buttonsControlUI.SetActive (false);
		zombieCounterUI.SetActive (false);
        reviveUI.SetActive(false);
	}

	IEnumerator AnimateNewWaveBanner(){
		float animatePercent = 0;
		float delayTime = 2f;
		float speed = 1.5f;
		float endDelayTime = Time.time + 1 / speed + delayTime;
		int dir = 1;

		while(animatePercent >= 0){
			animatePercent += Time.deltaTime * speed * dir;

			if (animatePercent >= 1) {
				animatePercent = 1;

				if(Time.time > endDelayTime){
					dir = -1;
				}
			}
				
			newWaveBanner.anchoredPosition = Vector2.up * Mathf.Lerp (450, 150, animatePercent);
			yield return null;
		}
	}

	IEnumerator Fade(Color from, Color to, float time){
		float speed = 2 / time;
		float percent = 0;

		while(percent < 1){
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp (from, to, percent);
			yield return null;
		}
	}

    IEnumerator FadeBloodyScreen(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1){
            percent += Time.deltaTime * speed;
            bloodImage.color = Color.Lerp(from, to, percent);
            yield return null;
        }
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            bloodImage.color = Color.Lerp(to, from, percent);
            yield return null;
        }
    }

    IEnumerator FadeText(Color from, Color to, float time)
    {

        while (isCoroutine)
        {
            float speed = 1 / time;
            float percent = 0;

            while (percent < 1)
            {
                percent += speed * Time.deltaTime;
                tapToCountinue.color = Color.Lerp(from, to, percent);
                yield return null;
            }
            yield return new WaitForSeconds(0.75f);
        }
    }

    public void StartNewGame(){
        ReviveButton.isReviveButtonPressed = false;
        SceneManager.LoadScene("GameWindow");
    }

	public void BackToMenu(){
        ReviveButton.isReviveButtonPressed = false;
        SceneManager.LoadScene ("Menu");
	}
}
