using UnityEngine;
using System.Collections;

public class Kongregate : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if ( !Application.isWebPlayer )
			return;
		
		// taken from the Kongregate docs: http://developers.kongregate.com/docs/api-overview/client-api
		string javaCode = @"
	// Load the API
	var kongregate;
	kongregateAPI.loadAPI(onComplete);

	// Callback function
	function onComplete(){
	// Set the global kongregate API object
	kongregate = kongregateAPI.getAPI();
	}
	";
		
		Application.ExternalEval( javaCode );
	}
	
	public static void SubmitScore(int score)
	{
		SubmitStat( "Score", score );
	}
	
	private static void SubmitStat(string stat, int statValue)
	{
		if ( !Application.isWebPlayer )
			return;
		
		string escapedStat = WWW.EscapeURL( stat );
		string javaCode = string.Format( "if (kongregate != null) kongregate.stats.submit(\'{0}\', {1});", escapedStat, statValue );
		
		Application.ExternalEval( javaCode );
	}


	// Update is called once per frame
	void Update () {
	
	}
}
