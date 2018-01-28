using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {

	public event System.Action<int> OnNewWave;
	public Wave[] waves;
	public ZombieController zombie;
	public GameObject buttonsControlUI;
	[HideInInspector]
	public static int zombiesRemainingAlive;
	Wave currentWave;
	int currentWaveNumber;
	int zombiesRemainingToSpawn;
	float nextSpawnTime;
	float timeBetweenCampingChecks = 2;
	float campThresholdDistance = 1.5f;
	float nextCampCheckTime;
	float timer;
	//float timeToSpawning;
	bool isCamping;
	bool isDisabledPlayerDeath;
	Vector3 campPositionOld;

	MapGenerator map;
	LivingEntity playerEntity;
	Transform playerTransform;

	void Start () {
		map = FindObjectOfType<MapGenerator> ();
		playerEntity = FindObjectOfType<PlayerController> ();
		playerTransform = playerEntity.transform;

		nextCampCheckTime = timeBetweenCampingChecks * Time.time;
		campPositionOld = playerTransform.position;
		playerEntity.OnDeath += OnPlayerDeath;
		//timeToSpawning = 5f; 
		//timer = 0f;

		NextWave ();
	}
	
	void Update () {
		//timer = Time.time;

		if(!isDisabledPlayerDeath){

			if(Time.time > nextCampCheckTime){
				nextCampCheckTime = Time.time + timeBetweenCampingChecks;
				isCamping = (Vector3.Distance(playerTransform.position, campPositionOld) < campThresholdDistance);
				campPositionOld = playerTransform.position;
			}

			//if (timer > timeToSpawning) {
				if((zombiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > nextSpawnTime && !GameUI.IsPlayerDead){
					zombiesRemainingToSpawn--;
					nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

					buttonsControlUI.SetActive (true);
					StartCoroutine (SpawnZombie());
				}
			//}
		}
	}

	IEnumerator SpawnZombie(){
		float spawnDelay = 2f;
		float tileFlashSpeed = 4f;
		Transform randomTile = map.GetRandomOpenTile ();

		if(isCamping){
			randomTile = map.GetTileFromPosition (playerTransform.position);
		}

		Material tileMaterial = randomTile.GetComponent<Renderer> ().material;
		Color initialColor = tileMaterial.color;
		Color flashColor = Color.red;
		float spawnTimer = 0;

		while(spawnTimer < spawnDelay){
			tileMaterial.color = Color.Lerp (initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1f));
			spawnTimer += Time.deltaTime;
			yield return null;
		}

		ZombieController spawnedZombie = Instantiate (zombie, randomTile.position + Vector3.up, Quaternion.identity) as ZombieController;
        //AudioManager.instance.PlaySound("Zombie sound", spawnedZombie.transform.position);
        spawnedZombie.OnDeath += OnZombieDeath;
		spawnedZombie.SetCharacteristics (currentWave.moveSpeed, currentWave.zombieHealth, currentWave.targetDamage);
	}

	void OnZombieDeath(){
		zombiesRemainingAlive--;
		if(zombiesRemainingAlive == 0){
			NextWave ();
		}
	}

	void OnPlayerDeath(){
		isDisabledPlayerDeath = true;
	}

	void ResetPlayerPosition(){
		playerTransform.position = map.GetTileFromPosition (Vector3.zero).position + Vector3.up * 3;
	}

	void NextWave(){
		if(currentWaveNumber > 0){
			AudioManager.instance.PlaySound2D ("Level completed");
		}
		currentWaveNumber++;
		if(currentWaveNumber - 1 < waves.Length){
			currentWave = waves[currentWaveNumber - 1];
			zombiesRemainingToSpawn = currentWave.zombieCount;
			zombiesRemainingAlive = zombiesRemainingToSpawn;

			if(OnNewWave != null){
				OnNewWave (currentWaveNumber);
			}
			ResetPlayerPosition ();
		}
	}

	[System.Serializable]
	public class Wave{
		public bool infinite;
		public int zombieCount;
		public float timeBetweenSpawns;

		public float moveSpeed;
		public float zombieHealth;
		public float targetDamage;
	}
}
