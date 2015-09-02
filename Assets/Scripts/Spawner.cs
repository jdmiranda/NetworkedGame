using UnityEngine;
using UnityEngine.Networking;

public class Spawner : NetworkBehaviour
{

	public GameObject alien;
	public int numAliens = 10;
	public int spread = 25;



	
	public override void OnStartServer()
	{
		for (int i=0; i < numAliens; i++)
		{
			Vector2 pos = new Vector2( Random.Range (-spread, spread), Random.Range (-spread, spread));
			NetworkServer.Spawn((GameObject)Instantiate(alien, pos, Quaternion.identity));
		}
	}
}
