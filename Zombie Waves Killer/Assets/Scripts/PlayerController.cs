using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : LivingEntity {

	public float runSpeed = 2;
	public float turnSmoothTime = 0.2f;
	public float speedSmoothTime = .1f;

    public VirtualJoystick joystick;

    private float turnSmoothVelocity;
    private float speedSmoothVelocity;
    private float currentSpeed;
    private static float medicamentHealingValue;
    private Animator animator;
    private bool isPlayerHealthBarFull;
    private GunController gunController;

    public static float MedicamentHealingValue
    {
        get
        {
            return medicamentHealingValue;
        }

        set
        {
            medicamentHealingValue = value;
        }
    }

    protected override void Start () {
		base.Start ();
		animator = GetComponent<Animator> ();
        gunController = GetComponent<GunController>();
        MedicamentHealingValue = 25;
        isPlayerHealthBarFull = false;
    }

    void Update () {
		Vector2 input = new Vector2 (joystick.Horizontal(), joystick.Vertical());
		Vector2 inputDir = input.normalized;

		if(inputDir != Vector2.zero){
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg;
			transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
		}

		float speed = (VirtualJoystick.isJoystickPressed) ? runSpeed : 0;

		currentSpeed = Mathf.SmoothDamp (currentSpeed, speed, ref speedSmoothVelocity, speedSmoothTime);
		transform.Translate (transform.forward * currentSpeed * Time.deltaTime, Space.World);

		float animationSpeedPercent = (VirtualJoystick.isJoystickPressed) ? 1 : 0;
		animator.SetFloat ("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);

        // revive player health bar when revive button is pressed
        if (ReviveButton.isReviveButtonPressed && !isPlayerHealthBarFull) {
            ReviveFullHealthBar();
            this.gameObject.transform.position = new Vector3(0, 0, 0);
            isPlayerHealthBarFull = true;
            //Debug.Log("health " + health);
        }

        if (Input.GetMouseButton(0) && ShootButton.isShootButtonPressed) {
            gunController.OnTriggerHold();
        }

        if (Input.GetMouseButtonUp(0)) {
            gunController.OnTriggerRelease();
        }
	}

    public override void Die(){
		AudioManager.instance.PlaySound ("Player death", transform.position);
		base.Die ();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collider>().tag == "Syringe") {
            MedicamentHealing(MedicamentHealingValue);
            Destroy(other.gameObject);
        }
    }
}
