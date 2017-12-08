using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuController : MonoBehaviour {
	public GameObject settingsMenu;
	public GameObject mainMenu;
	public GameObject credits;
	public GameObject settingsParamTitle;
	public InputField settingsParam;
	public GameObject dropdownLabel;
	public GameObject firstSelectSettings;
	public GameObject firstSelectMainMenu;
	public GameObject firstSelectCredits;
	public EventSystem eventSystem;
	private GameType gameType;

	public GameObject glu;
	public GameObject mm;

	public Slider slider;
	public Text sliderNum;

	void Start() {
		this.settingsMenu.SetActive(false);
		this.credits.SetActive(false);
		this.mainMenu.SetActive(true);
		if(!GameObject.Find("GameLoadUtils")) {
			var g = Instantiate(glu);
			g.name = "GameLoadUtils";
		}
		if(!GameObject.Find("MusicManager")) {
			var m = Instantiate(mm);
			m.name = "MusicManager";
		}
	}

	public void ClickStartGame() {
		Debug.Log("Loading Game");
		PlayerPrefs.SetString("NumPlayers", this.sliderNum.text);
		SceneManager.LoadScene("Game");
	}

	public void ClickSettingsMenu(GameObject label) {
		Debug.Log("Enabling Settings Menu");
		this.mainMenu.SetActive(false);
		this.settingsMenu.SetActive(true);
		GameTypeChanged(label);
		LoadCurrentSettings();
		eventSystem.SetSelectedGameObject(firstSelectSettings);
	}

	public void ClickCredits() {
		Debug.Log("Enabling Credits Menu");
		this.mainMenu.SetActive(false);
		this.credits.SetActive(true);
		eventSystem.SetSelectedGameObject(firstSelectCredits);
	}

	public void ReturnToMainMenu() {
		Debug.Log("Returning to Main Menu");
		this.settingsMenu.SetActive(false);
		this.credits.SetActive(false);
		this.mainMenu.SetActive(true);
		eventSystem.SetSelectedGameObject(firstSelectMainMenu);
	}

	public void GameTypeChanged(GameObject label) {
		Text text = label.GetComponent<Text>();
		this.settingsParam.contentType = InputField.ContentType.IntegerNumber;
		this.settingsParam.characterLimit = 3;
		if(text.text == "Time Based") {
			Text labelText = settingsParamTitle.GetComponent<Text>();
			labelText.text = "Time in Seconds";
			this.gameType = GameType.TIME;
		} else if(text.text == "Last Man Standing") {
			Text labelText = settingsParamTitle.GetComponent<Text>();
			labelText.text = "Last Man Standing";
			this.gameType = GameType.LIVES;
		} else if(text.text == "Number Of Kills") {
			Text labelText = settingsParamTitle.GetComponent<Text>();
			labelText.text = "Number Of Kills";
			this.gameType = GameType.KILLS;
		} else {
			Debug.LogError("Game Type Not Implemented!");
		}
	}

	public void SaveSettings(GameObject text) {
		if(this.gameType == GameType.TIME) {
			PlayerPrefs.SetString("GameType", "TIME");
		} else if(this.gameType == GameType.LIVES) {
			PlayerPrefs.SetString("GameType", "LIVES");
		} else if(this.gameType == GameType.KILLS) {
			PlayerPrefs.SetString("GameType", "KILLS");
		}
		Text txtValue = text.GetComponent<Text>();
		PlayerPrefs.SetInt("GameTypeParam", int.Parse(txtValue.text));
		ReturnToMainMenu();
	}

	public void ExitGame() {
		Application.Quit();
	}

	public void SliderNum() {
		this.sliderNum.text = this.slider.value.ToString();
	}

	public void LoadCurrentSettings() {
		string gameType = PlayerPrefs.GetString("GameType");
		int param = PlayerPrefs.GetInt("GameTypeParam");

		if(param != 0) {
			this.settingsParam.text = param.ToString();
		} else {
			this.settingsParam.text = "30";
		}

		if(gameType != "") {
			if(gameType == "KILLS") {
				this.settingsParamTitle.GetComponent<Text>().text = "Number Of Kills";
				this.dropdownLabel.GetComponent<Text>().text = "Number Of Kills";
			} else if(gameType == "LIVES") {
				this.settingsParamTitle.GetComponent<Text>().text = "";
				this.dropdownLabel.GetComponent<Text>().text = "Last Man Standing";
			} else {
				this.settingsParamTitle.GetComponent<Text>().text = "Time In Seconds";
				this.dropdownLabel.GetComponent<Text>().text = "Time In Seconds";
			}
		} else {
			this.settingsParamTitle.GetComponent<Text>().text = "Time In Seconds";
			this.dropdownLabel.GetComponent<Text>().text = "Time In Seconds";
		}
	}
}

	enum GameType {
		TIME,
		LIVES,
		KILLS
	}
