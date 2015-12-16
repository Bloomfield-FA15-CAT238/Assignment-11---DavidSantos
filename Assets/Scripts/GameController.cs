using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class GameController : MonoBehaviour {
	public static GameController gc;

	private static PlayerData playerData;
	private static UIManager ui;
	private static GameObject player;

    public AudioClip waterSound;
	public AudioClip coinSound;
	public AudioClip deathSound;
	public AudioClip keySound;

	#region Standard Unity Methods
	void Awake() {
		if (gc == null) {
			DontDestroyOnLoad (gameObject);
			gc = this;
			playerData = new PlayerData ();
			ui = gameObject.GetComponent<UIManager>();
			OnLevelWasLoaded();
		} else if(gc != this) {
			Destroy(gameObject);
		}
	}

	void Start() {
		StartNewGame ();
	}

	void Update () {
		// Load data by key press
		if (Input.GetKeyUp (KeyCode.F1)) {
			LoadPlayerData();
		}		
		// Save data by key press
		if (Input.GetKeyUp (KeyCode.F2)) {
			SavePlayerData();
		}
	}

	void OnLevelWasLoaded() {
		ui.FindUIComponents ();
		player = GameObject.FindGameObjectWithTag ("Player");
		// Start particle system for level! 1 = Rain, 2 = Leave, 3 = Snow...
	}
	#endregion

	void StartNewGame() {
		playerData.hasKey = false;
		playerData.hitPoints = 100;
		playerData.score = 0;
		playerData.deaths = 0;
		playerData.currentLevel = 1;
		StartLevel ();
	}

	void StartLevel() {
		playerData.hasKey = false;
		ui.HideKeyImage ();
		ui.HideMessage ();
		RespawnPlayer ();
	}

	void RespawnPlayer() {
		GameObject respawnPoint = GameObject.FindGameObjectWithTag ("Respawn");
	//	GameObject player = GameObject.FindGameObjectWithTag ("Player");
		player.gameObject.transform.position = respawnPoint.gameObject.transform.position;
		playerData.hitPoints = 100;
		ui.UpdateHUDText (playerData);
		ui.UpdateHealthBar (playerData);
	}

	public void FoundKey() {
		playerData.hasKey = true;
		ui.ShowKeyImage ();
//		player.GetComponent<AudioSource>().clip = keySound;
		player.gameObject.GetComponent<AudioSource>().PlayOneShot(keySound,7.0f);
	}

	public void FoundCoin() {
		playerData.score += 100;
		ui.UpdateHUDText (playerData);
	//	GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent<AudioSource>().clip = coinSound;
			player.GetComponent<AudioSource>().Play();

		//player.GetComponent<AudioSource>().PlayOneShot(coinSound, 2.0f);
	}

	public void DamagePlayer(int damage) {
		print ("DAMAGE PLAYER");
		playerData.hitPoints -= damage;
		ui.UpdateHealthBar (playerData);

		if (playerData.hitPoints <= 0) {
			playerData.deaths++;
			RespawnPlayer ();
	//		GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent<AudioSource>().clip = deathSound;
			player.GetComponent<AudioSource>().Play(0);

			//player.GetComponent<AudioSource>().PlayOneShot(deathSound,1);
		}
	}

	public void EnteredExit() {
		if (playerData.hasKey) {
			playerData.currentLevel = Application.loadedLevel + 1;
			Application.LoadLevel (Application.loadedLevel + 1);
        } else {
			ui.ShowMessage("You need to find the key before winning!",10.0f);
		}
	}

	#region Load/Save Player Data
	public bool DoesLoadPlayerDataExist() {
		bool result = false;
		if (File.Exists (Application.persistentDataPath + "/playerStats.dat")) {
			result = true;
		}
		return result;
	}

	public void LoadPlayerData() {
		if (DoesLoadPlayerDataExist()) {
			print ("LOADING FILE: " + Application.persistentDataPath + "/playerStats.dat");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream fs = File.Open (Application.persistentDataPath + "/playerStats.dat", FileMode.Open);
			playerData = (PlayerData)bf.Deserialize (fs);
			fs.Close ();
			print ("LOADED FILE: " + Application.persistentDataPath + "/playerStats.dat");
			if(Application.loadedLevel!=playerData.currentLevel) {
				Application.LoadLevel(playerData.currentLevel);
			}
		} else {
			print ("NO FILE TO LOAD...");
		}
	}

	public void SavePlayerData() {
		print ("SAVING PLAYER DATA...");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream fs = File.Create (Application.persistentDataPath + "/playerStats.dat");
		if (fs!=null) {
			bf.Serialize (fs, playerData);
			fs.Close ();
			print ("SAVE TO FILE: " + Application.persistentDataPath + "/playerStats.dat");
		} else {
			print ("Failed to create files!");
		}
	}
	#endregion
}
