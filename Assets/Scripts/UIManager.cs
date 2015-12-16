using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {
    public static UIManager ui;
	private Text hudText;
	private Slider healthBar;
	private Image keyIcon;
	private Text messageText;

	private float timeToHideMessage;

	void Update() {
		if (Time.fixedTime > timeToHideMessage) {
			HideMessage ();
		}
	}

	public void FindUIComponents() {
		if (messageText == null) {
			messageText = GameObject.Find("MessageText").GetComponent<Text>();
		}
		if (keyIcon == null) {
			keyIcon = GameObject.Find("KeyIcon").GetComponent<Image>();
		}
		if (hudText == null) {
			hudText = GameObject.Find("HUDText").GetComponent<Text>();
		}
		if (healthBar == null) {
			healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
		}  
	}

	public void ShowMessage(string message, float timeout) {
		if (messageText) {
			messageText.text = message;
			timeToHideMessage = Time.fixedTime + timeout;
			messageText.enabled = true;
		}
	}
	
	public void HideMessage() {
		if (messageText) {
			messageText.enabled = false;
		}
	}
	
	public void HideKeyImage() {
		if (keyIcon) {
			keyIcon.enabled = false;
		}
	}
	
	public void ShowKeyImage() {
		if (keyIcon) {
			keyIcon.enabled = true;
		}
	}
	
	public void UpdateHUDText(PlayerData playerData) {
		if (hudText) {
			string hud = "Score: " + playerData.score;
			hud += "\n" + "Level: " + playerData.currentLevel;
			if (playerData.deaths > 0) {
				hud += "\n" + "Deaths: " + playerData.deaths;
			}
			
			hudText.text = hud;
		}
	}
	
	public void UpdateHealthBar(PlayerData playerData) {
		if (healthBar) {
			healthBar.value = playerData.hitPoints;
		}
	}
}
