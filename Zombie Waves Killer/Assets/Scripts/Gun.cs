using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public enum FireMode { Auto, Burst, Single };
    public FireMode fireMode;
	public Transform[] muzzle;
	public Transform shell;
	public Transform shellEjection;
	public Bullet bullet;
	public float msBetweenShots = 300;
	public float muzzleVelocity = 50;
    public float reloadTime = .5f;
    public int burstCount;
    public int bulletsPerMagazine;
	public AudioClip shootAudio;
	public AudioClip reloadAudio;
	private float nextShotTime;
    private int shotsRemainingInBurst;
    private int bulletsRemainingInMagazine;
    private MuzzleFlash muzzleFlash;
    private Vector3 recoilSmoothDampVelocity;
    private bool triggerReleasedSinceLastShot;
    private bool isReloading;
    private const float RECOILTIMEVELOCITY = 0.2f;

	void Start(){
		muzzleFlash = GetComponent<MuzzleFlash> ();
        shotsRemainingInBurst = burstCount;
        bulletsRemainingInMagazine = bulletsPerMagazine;
	}

    private void LateUpdate()
    {
        // animate recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, RECOILTIMEVELOCITY);
        if (!isReloading && bulletsRemainingInMagazine == 0) {
            Reload();
        }
    }

    private void Shoot(){
		if(!isReloading && Time.time > nextShotTime && bulletsRemainingInMagazine > 0){
            if (fireMode == FireMode.Burst) {
                if (shotsRemainingInBurst == 0) {
                    return;
                }
                shotsRemainingInBurst--;
            } else if (fireMode == FireMode.Single) {
                if (!triggerReleasedSinceLastShot) {
                    return;
                }
            }

            for (int i = 0; i < muzzle.Length; i++) {
                if (bulletsRemainingInMagazine == 0)
                {
                    break;
                }
                bulletsRemainingInMagazine--;
                nextShotTime = Time.time + msBetweenShots / 1000;
                Bullet newBullet = Instantiate(bullet, muzzle[i].position, muzzle[i].rotation) as Bullet;
                newBullet.SetSpeed(muzzleVelocity);
            }
            
			Instantiate (shell, shellEjection.position, shellEjection.rotation);
			muzzleFlash.Activate ();
            transform.localPosition -= Vector3.back * RECOILTIMEVELOCITY;

			AudioManager.instance.PlaySound (shootAudio, transform.position);
		}
	}

    public void Reload() {
        if (!isReloading && bulletsRemainingInMagazine != bulletsPerMagazine) {
            StartCoroutine(AnimateReload());
            AudioManager.instance.PlaySound(reloadAudio, transform.position);
        }
    }

    IEnumerator AnimateReload() {
        isReloading = true;
        yield return new WaitForSeconds(.25f);

        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        float maxReloadAngle = 45;
        Vector3 initialRotation = transform.localEulerAngles;

        while (percent < 1) {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRotation + Vector3.up * reloadAngle;
            yield return null;
        }

        isReloading = false;
        bulletsRemainingInMagazine = bulletsPerMagazine;
    }

    public void OnTriggerHold() {
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease() {
        triggerReleasedSinceLastShot = true;
        shotsRemainingInBurst = burstCount;
    }
}
