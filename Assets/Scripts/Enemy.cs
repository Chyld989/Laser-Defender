using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[Header("Enemy")]
	[SerializeField] float HitPoints = 100;
	[SerializeField] float ShotCounter;
	[SerializeField] float minTimeBetweenShots = 0.2f;
	[SerializeField] float maxTimeBetweenShots = 3f;
	[Header("Projectile")]
	[SerializeField] GameObject ProjectilePrefab;
	[SerializeField] float ProjectileSpeed = 10f;
	[SerializeField] AudioClip ProjectileSound;
	[SerializeField] [Range(0, 1)] float ProjectileVolume = 0.75f;
	[SerializeField] GameObject StandardExplosion;
	[SerializeField] float StandardExplosionDuration = 1f;
	[SerializeField] AudioClip StandardExplosionSound;
	[SerializeField] [Range(0, 1)] float StandardExplosionVolume = 0.75f;

	// Start is called before the first frame update
	void Start() {
		ResetShotCounter();
	}

	private void ResetShotCounter() {
		ShotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
	}

	// Update is called once per frame
	void Update() {
		CountdownToShooting();
		Shoot();
	}

	private void CountdownToShooting() {
		ShotCounter -= Time.deltaTime;
	}

	private void Shoot() {
		if (ShotCounter <= 0f) {
			var projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity);
			projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -ProjectileSpeed);
			AudioSource.PlayClipAtPoint(ProjectileSound, Camera.main.transform.position, ProjectileVolume);
			ResetShotCounter();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		DealDamage(collision);
		DestroyWhenDead();
	}

	private void DealDamage(Collider2D collision) {
		DamageDealer damageDealer = collision.gameObject.GetComponent<DamageDealer>();
		if (damageDealer != null) {
			HitPoints -= damageDealer.GetDamage();
			damageDealer.Hit();
		}
	}

	private void DestroyWhenDead() {
		if (HitPoints <= 0) {
			var explosion = Instantiate(StandardExplosion, transform.position, transform.rotation);
			Destroy(gameObject);
			Destroy(explosion, StandardExplosionDuration);
			AudioSource.PlayClipAtPoint(StandardExplosionSound, Camera.main.transform.position, StandardExplosionVolume);
		}
	}
}
