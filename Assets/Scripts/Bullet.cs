using UnityEngine;
using UnityEngine.Networking;

public class Bullet : MonoBehaviour
{
	public int damage = 40;
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		var combat = collider.GetComponent<Combat>();
		if (combat != null)
		{
			combat.TakeDamage(damage);
		}
		Destroy(gameObject);		
	}

	void Start()
	{
//		Debug.Log("Bullet start:" + SpaceManager.Now());
	}
}
