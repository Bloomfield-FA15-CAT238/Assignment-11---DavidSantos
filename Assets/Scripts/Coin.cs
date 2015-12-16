using UnityEngine;

public class Coin : MonoBehaviour {
	Vector3 rotate = new Vector3(10.0f, 0.0f, 0.0f);

	void Update () {
		transform.Rotate( rotate );
	}
	
	void OnTriggerEnter(Collider other) {
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gc.FoundCoin();
		Destroy(this.gameObject);
	}
}