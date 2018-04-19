using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverCtrl : MonoBehaviour {

	public GameObject gameoverUI;
	public GameObject gamewinUI;

	private bool isGameOver=false;

	void OnGameOver(){
		OnGameEnd (gameoverUI);
	}

	void OnGameWin(){
		OnGameEnd (gamewinUI);
	}

	void OnGameEnd(GameObject gameUI){
		gameUI.SetActive (true);
		isGameOver = true;
		// remove event listener
		GuardCtrl.onSpotPlayer -= OnGameOver;
		FindObjectOfType<PlayerCtrl> ().onPlayerReachGoal -= OnGameWin;
	}

	// Use this for initialization
	void Start () {
		GuardCtrl.onSpotPlayer += OnGameOver;
		FindObjectOfType<PlayerCtrl> ().onPlayerReachGoal += OnGameWin;
	}
	
	// Update is called once per frame
	void Update () {
		if (isGameOver) {
			if (Input.GetKeyDown (KeyCode.Space)) {
				isGameOver = false;
				SceneManager.LoadScene ("stealth");
			}
		}
	}
}
