using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour {

	public Rigidbody shellRigidbody;
	public float forceMin;
	public float forceMax;
	float shellLifetime = 4f;
	float fadetime = 2f;

	void Start () {
		float force = Random.Range (forceMin, forceMax);
		shellRigidbody.AddForce (transform.right * force);
		shellRigidbody.AddTorque (Random.insideUnitSphere * force);

		StartCoroutine (ShallFade());
	}

	IEnumerator ShallFade(){
		yield return new WaitForSeconds (shellLifetime);

		float percent = 0;
		float fadeSpeed = 1 / fadetime;
		Material material = GetComponent<Renderer> ().material;
		Color initialColor = material.color;

		while(percent < 1){
			percent += Time.deltaTime * fadeSpeed;
			material.color = Color.Lerp (initialColor, Color.clear, percent);
			yield return null;
		}

		Destroy (gameObject);
	}
}
