using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game2048 : MonoBehaviour
{

		private const float CELL_MARGIN = 2.2f;
		private const int FOUR_CELL_PROBABILITY = 5; // 1 from 5
		private const int CELLS_W = 6;
		private const int CELLS_H = 4;
		private const string CELLS_S = ";";
		private const string MOVE_UP = "up";
		private const string MOVE_DOWN = "down";
		private const string MOVE_LEFT = "left";
		private const string MOVE_RIGHT = "right";
		private Dictionary<string, GameObject> cells = new Dictionary<string, GameObject> ();
		private bool mouseLeftDown = false;
		private bool moveSuccess = false;
		private bool moveInProgress = false;
		private Vector3 downStartPosition;
		private Vector3 downEndPosition;
		private Vector3 nullPosition = new Vector3 (0, 0, 0);
		public GameObject cellTemplate;
		private int playerScore = 0;
		private int playerScoreBest = 0;
		private TextMesh scoreText;
		private TextMesh scoreBestText;

		// Use this for initialization
		void Start ()
		{
				prepareBG ();
				addCells (2, false);

				GameObject score = GameObject.Find ("score");
				scoreText = score.GetComponentInChildren<TextMesh> ();
				GameObject scoreBest = GameObject.Find ("scoreBest");
				scoreBestText = scoreBest.GetComponentInChildren<TextMesh> ();
		}

		void prepareBG ()
		{
				GameObject cellBgTemplate;
				GameObject newObject;
				cellBgTemplate = GameObject.Find ("cellBg");
				Quaternion objectRot = cellBgTemplate.transform.rotation;
				for (int i = 0; i < CELLS_W; i++) {
						for (int j = 0; j < CELLS_H; j++) {
								Vector3 objectPos = cellBgTemplate.transform.position;
								objectPos.x += i * CELL_MARGIN;
								objectPos.y -= j * CELL_MARGIN;
								newObject = (GameObject)Object.Instantiate (cellBgTemplate, objectPos, objectRot);
						}
				}
				GameObject.Destroy (cellBgTemplate);
		}
	
		void addCells (int toAdd, bool fourCellToo)
		{
				GameObject newObject;
				Quaternion objectRot = cellTemplate.transform.rotation;

				int tryCount = 0;
				while (toAdd > 0) {
						//Add New
						int randX = Random.Range (0, CELLS_W);
						int randY = Random.Range (0, CELLS_H);
						string randKey = randX + CELLS_S + randY;
						if (!cells.ContainsKey (randKey)) {
								Vector3 objectPos = cellTemplate.transform.position;
								objectPos.x += randX * CELL_MARGIN;
								objectPos.y -= randY * CELL_MARGIN;
								newObject = (GameObject)Object.Instantiate (cellTemplate, objectPos, objectRot);
								if (fourCellToo && Random.Range (0, FOUR_CELL_PROBABILITY) == 1) 
										newObject.SendMessage ("doubleUp");
								cells.Add (randKey, newObject);
								toAdd--;
						}
						tryCount++;
						if (tryCount > CELLS_W * CELLS_H * 2) {
								Debug.Log ("Add false");
								break;
						}
				}

				//cells
		}

	
		// Update is called once per frame
		void Update ()
		{
				moveSuccess = false;
				if (Input.GetMouseButtonDown (0)) {
						if (!mouseLeftDown) {
								mouseLeftDown = true;
								downStartPosition = Input.mousePosition;
						}
				} else {
						if (mouseLeftDown && Input.GetMouseButtonUp (0)) {
								mouseLeftDown = false;
								downEndPosition = Input.mousePosition;
								if (downStartPosition != nullPosition && downEndPosition != nullPosition) {
										float deltaX = downStartPosition.x - downEndPosition.x;			
										float deltaY = downStartPosition.y - downEndPosition.y;	
										if (deltaX < -5) 
												calcMovement (MOVE_RIGHT);
										if (deltaX > 5) 
												calcMovement (MOVE_LEFT);
										if (deltaY > 1) 
												calcMovement (MOVE_DOWN);
										if (deltaY < -1) 
												calcMovement (MOVE_UP);
										downStartPosition = nullPosition;
										downEndPosition = nullPosition;
								}

							
						}
				
				}
				if (Input.GetKeyUp (KeyCode.UpArrow))
						calcMovement (MOVE_UP);
				if (Input.GetKeyUp (KeyCode.DownArrow))
						calcMovement (MOVE_DOWN);
				if (Input.GetKeyUp (KeyCode.RightArrow))
						calcMovement (MOVE_RIGHT);
				if (Input.GetKeyUp (KeyCode.LeftArrow))
						calcMovement (MOVE_LEFT);
		}

		void calcMovement (string side)
		{
				if (moveInProgress)
						return;
				moveInProgress = true;
				if (side == MOVE_RIGHT) {
						Debug.Log ("Move right");
						for (int i = CELLS_W-1; i >= 0; i--) {
								for (int j = 0; j < CELLS_H; j++) {
										calcStep (i, j, 1, 0);
								}
						}
				} else if (side == MOVE_LEFT) {
						Debug.Log ("Move left");
						for (int i = 0; i < CELLS_W; i++) {
								for (int j = 0; j < CELLS_H; j++) {
										calcStep (i, j, -1, 0);
								}
						}
				} else if (side == MOVE_DOWN) {
						Debug.Log ("Move down");
						for (int i = 0; i < CELLS_W; i++) {
								for (int j = CELLS_H-1; j >= 0; j--) {
										calcStep (i, j, 0, 1);
								}
						}
				} else if (side == MOVE_UP) {
						Debug.Log ("Move up");
						for (int i = 0; i < CELLS_W; i++) {
								for (int j = 0; j < CELLS_H; j++) {
										calcStep (i, j, 0, -1);
								}
						}
				}

				if (moveSuccess) {
						foreach (var item in cells.Values) {
								item.SendMessage ("positionAnim");
						}

						Invoke ("addNewCell", 0.2f);
						
				}
				Invoke ("moveEnd", 0.2f);			
				
		}

		void moveEnd ()
		{
				moveInProgress = false;
		}

		void addNewCell ()
		{
				addCells (1, true);
		}

		void calcStep (int i, int j, int addI, int addJ)
		{
				string key = i + CELLS_S + j;
				if (cells.ContainsKey (key))
						tryMove (cells [key], i, j, addI, addJ);
				if (cells.ContainsKey (key))
						tryUnion (cells [key], i, j, addI, addJ);
				if (cells.ContainsKey (key))
						tryMove (cells [key], i, j, addI, addJ);
		}

		void tryMove (GameObject gameObject, int i, int j, int addI, int addJ)
		{
				int newI = i + addI;
				int newJ = j + addJ;

				if (newJ >= 0 && newJ < CELLS_H && newI >= 0 && newI < CELLS_W) {
						string objectKey = i + CELLS_S + j;
						string key = newI + CELLS_S + newJ;
						if (!cells.ContainsKey (key)) {
								//gameObject.transform.position += new Vector3 (1.1f * addI, 1.1f * -addJ, 0);
								gameObject.SendMessage ("addPosition", new Vector3 (CELL_MARGIN * addI, CELL_MARGIN * -addJ, 0));
								//iTween.MoveTo(gameObject, iTween.Hash("y", gameObject.transform.position.y + 1.1f, "time", 0.5f, "easetype", iTween.EaseType.linear));
								cells.Remove (objectKey);
								cells.Add (key, gameObject);
								tryMove (gameObject, newI, newJ, addI, addJ);
								moveSuccess = true;
						}
				}
		}
	
		void tryUnion (GameObject cellToUnion, int i, int j, int addI, int addJ)
		{
				int newI = i + addI;
				int newJ = j + addJ;
		
				if (newJ >= 0 && newJ < CELLS_H && newI >= 0 && newI < CELLS_W) {
						string objectKey = i + CELLS_S + j;
						string key = newI + CELLS_S + newJ;
						if (cells.ContainsKey (key)) {
								GameObject nextCell = cells [key];
								TextMesh firstText = cellToUnion.GetComponentInChildren<TextMesh> ();
								TextMesh nextText = nextCell.GetComponentInChildren<TextMesh> ();
								
								if (nextText.text == firstText.text) {
										nextCell.SendMessage ("doubleUp");
										updateScore (playerScore + int.Parse (firstText.text) * 2);
										cells.Remove (objectKey);
										GameObject.Destroy (cellToUnion);
										moveSuccess = true;
								}
						}
				}
		}

		void updateScore (int scoreValue)
		{
				scoreText.text = scoreValue.ToString ();
				playerScore = scoreValue;
				if (playerScore > playerScoreBest) {
						playerScoreBest = playerScore;
						scoreBestText.text = scoreText.text;
				}
		}

		public void NewGame ()
		{
				foreach (var item in cells.Values) {
						GameObject.Destroy (item);
				}
				cells = new Dictionary<string, GameObject> ();
				addCells (2, false);
				updateScore (0);
		}

}
