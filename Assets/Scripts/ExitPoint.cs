using UnityEngine;
using System.Collections;

public class ExitPoint : MonoBehaviour {
	void OnTriggerEnter(Collider other) {
		GameController gc = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController> ();
		gc.EnteredExit();
    }
}
