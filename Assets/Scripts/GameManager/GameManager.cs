using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

public bool shouldEndGameOnTime = true;
public bool shouldGeneratePlayersViaGameManager = true;

public float roundTime = 30; //Time in sec
public GameObject playerPrefab;

[Range(1, 4)]
public static int numberOfPlayers;
public GameObject p1;
public GameObject p2;
public GameObject p3;
public GameObject p4;

public GameObject winningPlayer;

public Text p1UI;
public Text p2UI;
public Text p3UI;
public Text p4UI;
public Text timeUI;

private string gameType;
private float gameTypeParam;

static public GameManager gm;

public GameLoadUtils glu;

	/*
	* Runs when the object is created
	*/
	void Start () {
		this.winningPlayer = null;
		PlayerPrefs.DeleteKey("WinningPlayerName");
		this.gameType = PlayerPrefs.GetString("GameType");
		this.gameTypeParam = PlayerPrefs.GetInt("GameTypeParam");

		if(this.gameType == "TIME") {
			this.roundTime = this.gameTypeParam;
		} else {
			this.roundTime = 0;
		}

		Debug.Log(gameType);
		Debug.Log(gameTypeParam);

		gm = this;

		//Set players to ui by disabling player script by attaching
			numberOfPlayers = int.Parse(PlayerPrefs.GetString("NumPlayers"));
			switch(numberOfPlayers) {
				case 2:
					this.p3.SetActive(false);
					this.p4.SetActive(false);
					break;
				case 3:
					//Make 1 AI
					this.p4.SetActive(false);
					break;	
				default:
				break;
			}
	}
	

	/*
	* Runs every frame Update
	* Currently set up to decrement the game timer and check for the end game
	* state.
	*/
	void Update () {

		if(this.gameType == "TIME") {
			//Decrement Timer
			this.roundTime -= Time.deltaTime;
			if(this.shouldEndGameOnTime) {
				if(this.roundTime < 0) {
					SetWinningPlayerByMaxKills();
					EndGame();
				}
		}
		} else if(this.gameType == "KILLS") {
			SetWinningPlayerByMaxKillsGame();
			if(this.winningPlayer != null) {
				EndGame();
			}

			this.roundTime += Time.deltaTime;
		} else if(this.gameType == "LIVES") {
			SetWinningPlayerIfLastStanding();
			this.roundTime += Time.deltaTime;
		}
		UpdatePlayerScoreUI();
	}

	void UpdatePlayerScoreUI() {
		this.p1UI.text = "Hits: "+p1.GetComponent<Player>().countKills.ToString();
		this.p2UI.text = "Hits: "+p2.GetComponent<Player>().countKills.ToString();
		this.p3UI.text = "Hits: "+p3.GetComponent<Player>().countKills.ToString();
		this.p4UI.text = "Hits: "+p4.GetComponent<Player>().countKills.ToString();
		this.timeUI.text = "Time: "+Mathf.Round(this.roundTime).ToString();
	}

	void SetWinningPlayerByMaxKillsGame() {
			int p1Kills = this.p1.GetComponent<Player>().countKills;
			int p2Kills = this.p2.GetComponent<Player>().countKills;
			int p3Kills = this.p3.GetComponent<Player>().countKills;
			int p4Kills = this.p4.GetComponent<Player>().countKills;
			int maxKills = Mathf.Max(p1Kills, p2Kills, p3Kills, p4Kills);
			
			if(maxKills >= this.gameTypeParam && maxKills <= this.gameTypeParam) {
				if(p1Kills >= maxKills) {
					this.winningPlayer = p1;
					PlayerPrefs.SetString("WinningPlayerName", p1.name);
				} else if(p2Kills >= maxKills) {
					this.winningPlayer = p2;
					PlayerPrefs.SetString("WinningPlayerName", p2.name);
				} else if(p3Kills >= maxKills) {
					this.winningPlayer = p3;
					PlayerPrefs.SetString("WinningPlayerName", p3.name);
				} else if(p4Kills >= maxKills) {
					this.winningPlayer = p4;
					PlayerPrefs.SetString("WinningPlayerName", p4.name);
				}
			}
	}

	void SetWinningPlayerByMaxKills() {
			int p1Kills = this.p1.GetComponent<Player>().countKills;
			int p2Kills = this.p2.GetComponent<Player>().countKills;
			int p3Kills = this.p3.GetComponent<Player>().countKills;
			int p4Kills = this.p4.GetComponent<Player>().countKills;
			int maxKills = Mathf.Max(p1Kills, p2Kills, p3Kills, p4Kills);
			
			if(maxKills > 0) {
				if(p1Kills >= maxKills) {
					this.winningPlayer = p1;
					PlayerPrefs.SetString("WinningPlayerName", p1.name);
				} else if(p2Kills >= maxKills) {
					this.winningPlayer = p2;
					PlayerPrefs.SetString("WinningPlayerName", p2.name);
				} else if(p3Kills >= maxKills) {
					this.winningPlayer = p3;
					PlayerPrefs.SetString("WinningPlayerName", p3.name);
				} else if(p4Kills >= maxKills) {
					this.winningPlayer = p4;
					PlayerPrefs.SetString("WinningPlayerName", p4.name);
				}
			}
	}

	void SetWinningPlayerIfLastStanding() {
		List<GameObject> playerList = new List<GameObject>();
		playerList.Add(p1);
		playerList.Add(p2);
		playerList.Add(p3);
		playerList.Add(p4);

		List<GameObject> resultList = new List<GameObject>();
		resultList.Add(p1);
		resultList.Add(p2);
		resultList.Add(p3);
		resultList.Add(p4);

		foreach(GameObject player in playerList) {
			if(!player.activeSelf) {
				resultList.Remove(player);
			}
		}

		if(resultList.Count == 1) {
			this.winningPlayer = resultList[0];
			PlayerPrefs.SetString("WinningPlayerName", this.winningPlayer.name);
			EndGame();
		}


	}

	/*
	* Ends the game, cleans things up, loads game results scene.
	*/
	void EndGame() {
		SceneManager.LoadScene("Results");

		// clean up / save information
		// load the results scene.
	}

	public static void WaitAndRespawn(GameObject player, Vector3 pos){ 
		if(gm.gameType != "LIVES") {
			gm.StartCoroutine(gm.Respawn(player, pos));
		}
	}


	IEnumerator Respawn(GameObject player, Vector3 pos) {
		yield return new WaitForSeconds(3);
		player.transform.position = pos;
		player.GetComponent<Player>().isDead = false;
		player.SetActive(true);
	}

}
