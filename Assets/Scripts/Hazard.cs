using UnityEngine;

public class Hazard : MonoBehaviour {
	public int damagePoints = 5;
	public bool removeOnHit = true;

	void OnTriggerEnter(Collider other) {
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gc.DamagePlayer(damagePoints);
		if (removeOnHit) {
			Destroy (this.gameObject);
		}
	}
}
