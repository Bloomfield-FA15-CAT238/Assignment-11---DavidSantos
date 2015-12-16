using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour {
	public GameObject enemyToSpawn;

	private float timeToGenerate = 0.0f;
	
	void Update() {
		if (Time.fixedTime > timeToGenerate) {
			ChooseNewTime();
			Instantiate(enemyToSpawn,gameObject.transform.position, gameObject.transform.rotation);
		}
	}

	void ChooseNewTime() {
		timeToGenerate = Time.fixedTime + Random.Range (3, 10);
	}
}
