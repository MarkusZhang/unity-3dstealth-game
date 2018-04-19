using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuCtrl : MonoBehaviour {

	// Use this for initialization
	public void startGame(){
		SceneManager.LoadScene ("stealth");
	}
}
