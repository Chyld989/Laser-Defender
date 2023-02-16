using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[SerializeField] List<WaveConfig> WaveConfigs;
	[SerializeField] int StartingWave = 0;
	[SerializeField] bool loopWaves = false;

	// Start is called before the first frame update
	IEnumerator Start() {
		do {
			yield return StartCoroutine(SpawnAllWaves());
		} while (loopWaves);
	}

	private IEnumerator SpawnAllWaves() {
		for (int wave = StartingWave; wave < WaveConfigs.Count; wave++) {
			var currentWave = WaveConfigs[wave];
			yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
		}
	}

	private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig) {
		for (int enemy = 0; enemy < waveConfig.GetNumberOfEnemies(); enemy++) {
			var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetPathWaypoints()[0].transform.position, Quaternion.identity);
			newEnemy.GetComponent<EnemyPathing>().SetWaveConfig(waveConfig);
			yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
		}
	}
}
