using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject {
	[SerializeField] GameObject EnemyPrefab;
	[SerializeField] GameObject PathPrefab;
	[SerializeField] float TimeBetweenSpawns = 0.5f;
	[SerializeField] float SpawnRandomFactor = 0.3f;
	[SerializeField] int NumberOfEnemies = 5;
	[SerializeField] float MoveSpeed = 2f;

	public GameObject GetEnemyPrefab() {
		return EnemyPrefab;
	}

	public List<Transform> GetPathWaypoints() {
		var pathWaypoints = new List<Transform>();

		foreach (Transform child in PathPrefab.transform) {
			pathWaypoints.Add(child);
		}

		return pathWaypoints;
	}

	public float GetTimeBetweenSpawns() {
		return Random.Range(TimeBetweenSpawns - SpawnRandomFactor, TimeBetweenSpawns + SpawnRandomFactor);
	}

	public float GetSpawnRandomFactor() {
		return SpawnRandomFactor;
	}

	public int GetNumberOfEnemies() {
		return NumberOfEnemies;
	}

	public float GetMoveSpeed() {
		return MoveSpeed;
	}
}
