using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicamentsSpawner : MonoBehaviour {
	public GameObject medicamentPrefab;
	private bool isInstantiateCompleted;
	private int timeDelay = 1;
	private const float DISTANCEBETWEENTILES = 2f;
	//private Transform[] medicamentPosition = {Vector3}
	private Vector3[] positionArray = new [] { Vector3.right, Vector3.left, Vector3.back, Vector3.forward };
	MapGenerator map;

	void Start () {
		isInstantiateCompleted = false;
		map = FindObjectOfType<MapGenerator> ();
	}

	private void Update(){
		if(!isInstantiateCompleted && Time.time > timeDelay){
			for(int i = 0; i <= Random.Range(1, 4); i++){
				MedicamentInstantiate ();
			}
			isInstantiateCompleted = true;
		}
	}

	private void MedicamentInstantiate(){
		Transform randomTile = map.GetRandomOpenTile();

		Instantiate (medicamentPrefab, randomTile.position + positionArray[Random.Range(0,3)] * DISTANCEBETWEENTILES + Vector3.up, Quaternion.identity);
	}
}
