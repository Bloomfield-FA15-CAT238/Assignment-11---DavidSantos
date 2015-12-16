using UnityEngine;

public class Key : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gc.FoundKey ();
		Destroy(gameObject);
	}
}
