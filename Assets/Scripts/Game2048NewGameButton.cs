using UnityEngine;
using System.Collections;

public class Game2048NewGameButton : MonoBehaviour {
	private GameObject game2048;

	// Use this for initialization
	void Start () {
		game2048 = GameObject.Find ("Game2048");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnMouseUp()
	{
		game2048.SendMessage("NewGame");
		Debug.Log("new game");
	}

}
