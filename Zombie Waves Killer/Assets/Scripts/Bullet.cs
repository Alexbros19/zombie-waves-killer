using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
	public LayerMask collisionMask;
    public Color trailColor;
	float speed = 10;
	float damage = 1;
	float lifetime = 3;

	void Start(){
		Destroy (gameObject, lifetime);
        GetComponent<TrailRenderer>().material.SetColor("_TintColor", trailColor);
	}

	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}

	void Update () {
		float moveDistance = speed * Time.deltaTime;
		CheckCollisions (moveDistance);
		transform.Translate (Vector3.back * moveDistance);
	}

	void CheckCollisions(float moveDistance){
		Ray ray = new Ray (transform.position, transform.forward);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, moveDistance, collisionMask, QueryTriggerInteraction.Collide)){
			OnHitObject (hit.collider, hit.point);
		}
	}

	void OnHitObject(Collider c, Vector3 hitPoint){
		IDamageble damageableObject = c.GetComponent<IDamageble> ();
		if(damageableObject != null){
			damageableObject.TakeHit (damage, hitPoint, transform.forward);
		}
		GameObject.Destroy (gameObject);
	}
}
