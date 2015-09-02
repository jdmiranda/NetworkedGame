using UnityEngine;
using UnityEngine.Networking;

public class SlimeAlien : NetworkBehaviour
{
	public GameObject playerTarget;

	void FixedUpdate()
	{
		if (!isServer)
			return;

		if (playerTarget != null)
		{
			Vector2 diff = (Vector2)(playerTarget.transform.position - transform.position).normalized;
			
			GetComponent<Rigidbody2D>().AddForce(diff * Time.fixedDeltaTime * 20);
		}
		
	}
	void Update ()
	{
		if (!isServer)
			return;

		if (Random.Range(0,20) != 0)
		{
			Vector2 v2 = new Vector2(Random.Range(-10,11)*0.1f, Random.Range(-10,11)*0.1f);
			GetComponent<Rigidbody2D>().AddForce(v2*2);
			GetComponent<Rigidbody2D>().AddTorque(Random.Range(-10,10)*0.01f);
		}
			
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 20);
		foreach (var collider in colliders)
		{
			var controls = collider.gameObject.GetComponent<Controls>(); 
			if (controls != null)
			{
				playerTarget = collider.gameObject;
				break;
			}
		}
	}
	
	void OnCollisionEnter2D(Collision2D collider)
	{
		var controls = collider.gameObject.GetComponent<Controls>(); 
		if (controls != null)
		{
			// hit a player
			var combat = collider.gameObject.GetComponent<Combat>();
			combat.TakeDamage (10);
			
			GetComponent<Combat>().TakeDamage (1000);
		}
	}
	
	void OnDestroy()
	{
		GameManager.singleton.score += 1;
	}
}
