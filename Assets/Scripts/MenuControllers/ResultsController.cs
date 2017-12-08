using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsController : MonoBehaviour {

	public GameObject winnerText;

	void Start()
	{
		string winnerName = PlayerPrefs.GetString("WinningPlayerName");
		if(winnerName != "") {
			this.winnerText.GetComponent<Text>().text = PlayerPrefs.GetString("WinningPlayerName") + " Wins!";
		} else {
			this.winnerText.GetComponent<Text>().text = "It's a tie!";
		}
	}

	public void LoadMainMenu() {
		SceneManager.LoadScene("MainMenu");
	}

	public void LoadGameAgain() {
		SceneManager.LoadScene("Game");
	}
}
