using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivingEntity : MonoBehaviour, IDamageble {
    
	public float startingHealth;
	// event for calling next waves
	public event System.Action OnDeath;
    public event System.Action OnPlayerDeath;
	protected float health = 0f;
	protected bool dead;
    [SerializeField]
    private BarStat barStat;
    private static bool isPlayerDead;
    private const int REVIVEHEALTHBARVALUE = 100;

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

    private void Awake()
    {
        barStat.Initialize();
        IsPlayerDead = false;
    }

    protected virtual void Start(){
		health = startingHealth;
	}

	public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection){
		TakeDamage (damage);
	}

	public virtual void TakeDamage(float damage){
		health -= damage;

		if(health <= 0 && !dead){
			Die ();
		}
	}

	public virtual void TakeZombieDamage(float damage){
		health -= damage;

        if (health < 0){
            health = 0;
        }

        barStat.CurrentValue -= ZombieController.ZombieDamage;

		if(health <= 0 && !dead){
            //Die ();
            PlayerDie();
		}
	}

    public void PlayerDie() {
        ZombieController.attackDistanceThreshold = 0;
        IsPlayerDead = true;

        if (OnPlayerDeath != null) {
            OnPlayerDeath();
        }
    }

    public virtual void MedicamentHealing(float healingValue) {
        health += healingValue;

        barStat.CurrentValue += PlayerController.MedicamentHealingValue;
    }

    public void ReviveFullHealthBar() {
        if (health <= 0) {
            health += REVIVEHEALTHBARVALUE;
            barStat.CurrentValue += REVIVEHEALTHBARVALUE;
        }
    }

	public virtual void Die(){
		dead = true;
		if(OnDeath != null){
			OnDeath ();
		}
		GameCamera.Destroy (gameObject);
	}
}
