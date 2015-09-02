using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public int score = 0;
	
	public static GameManager singleton;
	
	void Awake()
	{
		singleton = this;
	}
	
	void OnGUI()
	{
		if (score == 10)
		{
			if (GUI.Button(new Rect(10,10,200,20), "YOU WIN!"))
			{
				Application.LoadLevel(0);
				//UnityEngine.Networking.NetworkManager.singleton.ServerChangeScene(0);
			}
			
		}
		else
		{
			GUI.Label(new Rect(10,10,200,20), "Score: " + score);
		}
	}
}
