using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ZombieController : LivingEntity {

	public enum State{Idle, Chasing, Attacking};
	State currentState;

	public float speedSmoothTime = .1f;
	public float zombieSpeed = 2f;
	public static float attackDistanceThreshold = 1.5f;
	public ParticleSystem zombieDeathEffect;
	public static event System.Action OnDeathStatic;

    private static int zombiesCounter;
    private float damage = 1f;
    private static float zombieDamage;
    private static float timerForBloodyScreen;
    private float timeBetweenAttacks = 1;
    private float nextAttackTime;
    private float zombieCollisionRadius;
    private float targetCollisionRadius;
    private bool hasTarget;
    private static bool isZombieAttack;
    private Transform target;
    private LivingEntity targetEntity;
    private UnityEngine.AI.NavMeshAgent pathfinder;
    private Animator animator;

    public static float ZombieDamage{
        get{ return zombieDamage; }
        set{ zombieDamage = value; }
    }

    public static bool IsZombieAttack
    {
        get
        {
            return isZombieAttack;
        }

        set
        {
            isZombieAttack = value;
        }
    }

    public static float TimerForBloodyScreen
    {
        get
        {
            return timerForBloodyScreen;
        }

        set
        {
            timerForBloodyScreen = value;
        }
    }

    public static int ZombiesCounter
    {
        get
        {
            return zombiesCounter;
        }

        set
        {
            zombiesCounter = value;
        }
    }

    void Awake(){
        //PlayerPrefs.DeleteAll();

        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent> (); 

		if (GameObject.FindGameObjectWithTag ("Player") != null) {
			hasTarget = true;
			animator = GetComponent<Animator> ();
			target = GameObject.FindGameObjectWithTag ("Player").transform;
			targetEntity = target.GetComponent<LivingEntity> ();
	
			zombieCollisionRadius = GetComponent<SphereCollider> ().radius;
			targetCollisionRadius = target.GetComponent<CapsuleCollider> ().radius;
		}
	}

	protected override void Start(){
		// call start method from living entity class
		base.Start ();

        IsZombieAttack = false;
        TimerForBloodyScreen = 0;
        attackDistanceThreshold = 1.5f;
        ZombiesCounter = PlayerPrefs.GetInt("zombiescount");

        if (hasTarget){
			currentState = State.Chasing;
			targetEntity.OnDeath += OnTargetDeath;

			StartCoroutine (UpdatePath());
		}
	}

    private void Timer()
    {
        TimerForBloodyScreen++;
    }

    public void SetCharacteristics(float moveSpeed, float zombieHealth, float targetDamage){
		if(!dead){
			pathfinder.speed = zombieSpeed;	
		}

		if(hasTarget){
			ZombieDamage = Mathf.Ceil (targetEntity.startingHealth / Random.Range(12, 16));
        }

		startingHealth = zombieHealth;
		targetDamage = damage;
	}
    // call function when zombie is taking damage
    public override void TakeHit (float damage, Vector3 hitPoint, Vector3 hitDirection)
	{
        // increase zombies counter and save its
        ZombiesCounter++;
        PlayerPrefs.SetInt("zombiescount", ZombiesCounter);

        AudioManager.instance.PlaySound ("Impact", transform.position);
		if(damage >= health){
			if(OnDeathStatic != null){
				OnDeathStatic ();
			}

			AudioManager.instance.PlaySound ("Enemy death", transform.position);
			Destroy(Instantiate (zombieDeathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.back, hitDirection)) as GameObject, zombieDeathEffect.startLifetime);
		}
		base.TakeHit (damage, hitPoint, hitDirection);
	}

	void OnTargetDeath(){
        IsPlayerDead = true;
		hasTarget = false;
		currentState = State.Idle;
	}

	public void Update(){
		if(hasTarget){
			if(Time.time > nextAttackTime){
				float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
				if(sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + zombieCollisionRadius + targetCollisionRadius, 2)){
					nextAttackTime = Time.time + timeBetweenAttacks;
					AudioManager.instance.PlaySound ("Enemy atack", transform.position);
					StartCoroutine (Attack());
                    IsZombieAttack = true;
                    InvokeRepeating("Timer", 0f, 1f);
                }
			}

			if(!dead){
				//pathfinder.speed = zombieSpeed;
				animator.SetFloat ("speedPercent", 1f, speedSmoothTime, Time.deltaTime);
			}
		}
	}

	IEnumerator Attack(){
		currentState = State.Attacking;
		pathfinder.enabled = false;

		Vector3 originalPosition = transform.position;
		Vector3 dirToTarget = (target.position - transform.position).normalized;
		Vector3 attackPosition = target.position - dirToTarget * (zombieCollisionRadius + targetCollisionRadius * 1.5f);
		float attackSpeed = 2;
		float percent = 0;
		bool hasAppliedDamage = false;

		//targetSkinMaterial.color = Color.red;

		while(percent <= 1){
			if(percent >= .5f && !hasAppliedDamage){
				hasAppliedDamage = true;
				targetEntity.TakeZombieDamage (ZombieDamage);
            }

			percent += Time.deltaTime * attackSpeed;
			// for attack and back and again attack
			float interpolation = (-Mathf.Pow (percent, 2) + percent) * 4;
			transform.position = Vector3.Lerp (originalPosition, attackPosition, interpolation);
			yield return null;
		}

		//targetSkinMaterial.color = targetOriginalColor;
		currentState = State.Chasing;
		pathfinder.enabled = true;
	}

    // for update zombie path N times on second instead 60 in Update() method
    IEnumerator UpdatePath(){
		float refreshRate = .01f;

		while(hasTarget){
			if(currentState == State.Chasing){
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				Vector3 targetPosition = target.position - dirToTarget * (zombieCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);

				if(!dead){
					pathfinder.SetDestination (targetPosition); 
				}
			}
			yield return new WaitForSeconds (refreshRate);
		}
	}
}
