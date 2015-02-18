using UnityEngine;
using System.Collections;

public class Cell2048 : MonoBehaviour
{

		private Renderer cellRenderer;
		private Vector3 addPos = new Vector3 (0, 0, 0);
		// Use this for initialization
		void Start ()
		{
				startAnim ();
		}

		void startAnim ()
		{
				GameObject quad = transform.Find ("Quad").gameObject;
				//quad.transform.localScale = new Vector3 ();
				//iTween.ScaleTo (quad, iTween.Hash ("scale", new Vector3 (1, 1, 1), "time", 0.2f, "easetype", iTween.EaseType.easeInSine));
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void addPosition (Vector3 vector)
		{
				addPos += vector;
		}

		public void positionAnim ()
		{
				//gameObject.transform.position += addPos;
				iTween.MoveTo (gameObject, iTween.Hash (
			"x", gameObject.transform.position.x + addPos.x, 
			"y", gameObject.transform.position.y + addPos.y, 
			"z", gameObject.transform.position.z + addPos.z, 
			"time", 0.2f, "easetype", iTween.EaseType.easeOutSine));
				addPos.Set (0, 0, 0);
		}

		public void doubleUp ()
		{
				cellRenderer = GetComponentInChildren<Renderer> ();
				TextMesh nextText = GetComponentInChildren<TextMesh> ();

				int newVal = int.Parse (nextText.text) * 2;
				nextText.text = newVal.ToString ();
				iTween.ScaleTo (nextText.gameObject, iTween.Hash ("scale", new Vector3 (1.5f, 1.5f, 1.5f), "time", 0.25f, "easetype", iTween.EaseType.easeInOutSine));
				iTween.ScaleTo (nextText.gameObject, iTween.Hash ("scale", new Vector3 (1f, 1f, 1f), "delay", 0.25f, "time", 0.25f, "easetype", iTween.EaseType.easeInOutSine));
				switch (nextText.text) {
				case "4":
						cellRenderer.material.color = new Color32 (238, 223, 200, 255);
						nextText.color = new Color32 (117, 111, 99, 255);
						break;
				case "8":
						cellRenderer.material.color = new Color32 (242, 177, 121, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						break;
				case "16":
						cellRenderer.material.color = new Color32 (245, 149, 99, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						break;
				case "32":
						cellRenderer.material.color = new Color32 (245, 124, 95, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						break;
				case "64":
						cellRenderer.material.color = new Color32 (245, 97, 61, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						break;
				case "128":
						cellRenderer.material.color = new Color32 (237, 206, 113, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						nextText.characterSize = 0.16f;
						break;
				case "256":
						cellRenderer.material.color = new Color32 (236, 203, 96, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						nextText.characterSize = 0.16f;
						break;
				case "512":
						cellRenderer.material.color = new Color32 (235, 198, 83, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						nextText.characterSize = 0.16f;
						break;
				case "1024":
						cellRenderer.material.color = new Color32 (236, 196, 62, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						nextText.characterSize = 0.14f;
						break;
				case "2048":
						cellRenderer.material.color = new Color32 (238, 194, 46, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						nextText.characterSize = 0.14f;
						break;
				case "4096":
						cellRenderer.material.color = new Color32 (61, 58, 51, 255);
						nextText.color = new Color32 (249, 244, 240, 255);
						nextText.characterSize = 0.14f;
						break;
				}

		}

}
