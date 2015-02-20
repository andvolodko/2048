using UnityEngine;
using System.Collections;

public class Game6X4Button : MonoBehaviour {

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
		Game2048.CELLS_W = 6;
		Game2048.CELLS_H = 4;
		game2048.SendMessage("NewGame");
		Debug.Log("new game");
	}
}
