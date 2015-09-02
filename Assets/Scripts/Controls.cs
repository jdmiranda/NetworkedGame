using UnityEngine;
using UnityEngine.Networking;

public class Controls : NetworkBehaviour {

	[SyncVar]
	float thrusting;
	float spin;
	
	int oldThrust;
	int oldSpin;
	

	float rotateSpeed = 200f;
	float acceleration = 8f;
	float bulletSpeed = 12f;
	
	public GameObject bulletPrefab;
	
	public ParticleSystem thruster1;
	public ParticleSystem thruster2;
	
	void FixedUpdate()
	{
		if (!isLocalPlayer)
			return;

		// update thrust
		if (thrusting != 0)
		{
			Vector3 thrustVec = transform.right * thrusting * acceleration;
			GetComponent<Rigidbody2D>().AddForce(thrustVec);
		}
		
		// update rotation 
		float rotate = spin * rotateSpeed;
		GetComponent<Rigidbody2D>().angularVelocity = rotate;
	}
	
	void Update ()
	{
		if (!isLocalPlayer)
			return;

		// movement
		int newSpin = 0;
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			newSpin += 1;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			newSpin -= 1;
		}
		
		int newThrust = 0;
		if (Input.GetKey(KeyCode.UpArrow))
		{
			newThrust += 1;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			newThrust -= 1;
		}		
		
		if (oldThrust != newThrust || oldSpin != newSpin)
		{
			DoThrust (newThrust, newSpin);
			oldThrust = newThrust;
			oldSpin = newSpin;
		}
		
		// fire
		if (Input.GetKeyDown(KeyCode.Space))
		{
//			SpaceManager.Reset();
			//Debug.Log("CmdDoFire");
			CmdDoFire(transform.position, GetComponent<Rigidbody2D>().rotation);
		}
		
		// center camera..
		Vector3 pos = transform.position;
		pos.z = -10;
		Camera.main.transform.position = pos;
	}
	
	void DoThrust(int newThrust, int newSpin)
	{
		// turn thrusters on and off
		if (newThrust == 0)
		{
			thruster1.Stop();
			thruster2.Stop();
		}
		else 
		{
			Quaternion rot;
			if (newThrust > 0)
			{
				rot = Quaternion.Euler(0,0,180);
			}
			else
			{
				rot = Quaternion.Euler(0,0,0);
			}
			thruster1.transform.localRotation = rot;
			thruster1.Play();
			thruster2.transform.localRotation = rot;
			thruster2.Play();
		}
		
		// apply new values
		this.thrusting = newThrust;
		this.spin = newSpin;
	}
	
	[Command]
	void CmdDoFire(Vector3 pos, float rotation)
	{
//		Debug.Log("CmdDoFire invoke:" + SpaceManager.Now());
		GameObject bullet = (GameObject)Instantiate(
			bulletPrefab, 
			pos + transform.right,
			Quaternion.Euler(0,0,rotation));
			
		var bullet2D = bullet.GetComponent<Rigidbody2D>();
		bullet2D.velocity = bullet2D.transform.right * bulletSpeed;
		Destroy(bullet, 2.0f);

		NetworkServer.Spawn(bullet);
	}


	/*
	public override void OnDeSerializeVars(NetworkReader reader, int channelId, bool initialState)
	{
		if (initialState)
		{
			this.thrusting = reader.ReadSingle();
			return;
		}

		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.thrusting = reader.ReadSingle();
		}
	}*/
	
}
