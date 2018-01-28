using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	private Vector3 cameraTarget;
	private Transform target;

	void Start () {

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

	void FixedUpdate () {
        
        if (GameObject.FindGameObjectWithTag ("Player") != null){

			cameraTarget = new Vector3 (target.position.x, transform.position.y, target.position.z);
			transform.position = Vector3.Lerp (transform.position, cameraTarget, Time.fixedDeltaTime * 8f);
		}
	}
}
