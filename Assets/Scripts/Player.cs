using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField] Level Level;
	[Header("Player")]
	[SerializeField] float HitPoints = 1000f;
	[SerializeField] float MoveSpeed = 10f;
	[SerializeField] AudioClip ExplosionSound;
	[SerializeField] [Range(0, 1)] float ExplosionVolume = 0.75f;
	[Header("Projectile")]
	[SerializeField] GameObject ProjectilePrefab;
	[SerializeField] float ProjectileFiringDelay = 0.125f;
	[SerializeField] float ProjectileSpeed = 20f;
	[SerializeField] AudioClip ProjectileSound;
	[SerializeField] [Range(0, 1)] float ProjectileVolume = 0.25f;

	Coroutine FiringCoroutine;

	float XMin;
	float XMax;
	float YMin;
	float YMax;
	AudioSource AudioSource;

	// Start is called before the first frame update
	void Start() {
		SetMoveBoundaries();
	}

	private void SetMoveBoundaries() {
		Camera gameCamera = Camera.main;
		SpriteRenderer playerShipSpriteRenderer = FindObjectOfType<SpriteRenderer>();
		XMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + (playerShipSpriteRenderer.size.x / 2);
		XMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - (playerShipSpriteRenderer.size.x / 2);
		YMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + (playerShipSpriteRenderer.size.y / 2);
		YMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - (playerShipSpriteRenderer.size.y / 2);
	}

	// Update is called once per frame
	void Update() {
		Move();
		Fire();
	}

	private void Move() {
		var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed;
		var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed;
		var newXPosition = Mathf.Clamp(transform.position.x + deltaX, XMin, XMax);
		var newYPosition = Mathf.Clamp(transform.position.y + deltaY, YMin, YMax);
		transform.position = new Vector2(newXPosition, newYPosition);
	}

	private void Fire() {
		if (Input.GetButtonDown("Fire1")) {
			if (FiringCoroutine == null) {
				FiringCoroutine = StartCoroutine(FireContinuously());
			}
		}
		if (Input.GetButtonUp("Fire1")) {
			if (FiringCoroutine != null) {
				StopCoroutine(FiringCoroutine);
				FiringCoroutine = null;
			}
		}
	}

	private IEnumerator FireContinuously() {
		while (true) {
			var projectile = Instantiate(ProjectilePrefab, transform.position, Quaternion.identity) as GameObject;
			projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0, ProjectileSpeed);
			AudioSource.PlayClipAtPoint(ProjectileSound, Camera.main.transform.position, ProjectileVolume);
			yield return new WaitForSeconds(ProjectileFiringDelay);
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
			AudioSource.PlayClipAtPoint(ExplosionSound, Camera.main.transform.position, ExplosionVolume);
			Destroy(gameObject);
			Level.LoadGameOver();
		}
	}
}
