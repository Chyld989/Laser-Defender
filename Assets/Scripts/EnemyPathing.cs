using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour {
	WaveConfig WaveConfig;
	List<Transform> Waypoints;
	int WaypointIndex = 0;

	// Start is called before the first frame update
	void Start() {
		Waypoints = WaveConfig.GetPathWaypoints();
		transform.position = Waypoints[WaypointIndex].transform.position;
	}

	// Update is called once per frame
	void Update() {
		MoveShip();
	}

	public void SetWaveConfig(WaveConfig waveConfig) {
		WaveConfig = waveConfig;
	}

	private void MoveShip() {
		if (WaypointIndex <= Waypoints.Count - 1) {
			var targetPosition = Waypoints[WaypointIndex].transform.position;
			var movementThisFrame = WaveConfig.GetMoveSpeed() * Time.deltaTime;

			transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

			if (transform.position == Waypoints[WaypointIndex].transform.position) {
				WaypointIndex++;
			}
		} else {
			Destroy(gameObject);
		}
	}
}
