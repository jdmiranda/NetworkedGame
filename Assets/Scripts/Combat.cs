using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour {
	
	[SyncVar(hook="OnDamage")]
	public int health = 100;

	public string playerName = "ThePilot";
	public bool canRespawn = true;
	
	public Texture box;
	public GameObject explosionPrefab;
	
	void OnDamage(int newHealth)
	{
		if (newHealth < 100)
		{
			MakeExplosion(0.3f);
		}
		health = newHealth;
	}

	public override void OnNetworkDestroy()
	{
		MakeExplosion(1.6f);
	}

	public void TakeDamage(int amount)
	{
		if (!isServer)
			return;

		health = health - amount;
		
		if (health <= 0)
		{
			if (canRespawn)
			{
				health = 100;

				RpcRespawn ();
			}
			else
			{
				Destroy (gameObject);
			}
		}
	}
	
	void MakeExplosion(float length)
	{
		GameObject exp = (GameObject)Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Destroy(exp, length);
	}
	
	[ClientRpc]
	void RpcRespawn()
	{
		MakeExplosion(2.0f);
		if (isLocalPlayer)
		{
			transform.position = Vector3.zero;
			GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			GetComponent<Rigidbody2D>().angularVelocity = 0;	
		}
	}
	
	void OnGUI()
	{
		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
		
		// draw the name with a shadow (colored for buf)	
		GUI.color = Color.black;
		GUI.Label(new Rect(pos.x-20, Screen.height - pos.y - 40, 100, 30), playerName);
		
		GUI.color = Color.white;
		GUI.Label(new Rect(pos.x-21, Screen.height - pos.y - 41, 100, 30), playerName);		
		
		// draw health bar background
		GUI.color = Color.grey;
		GUI.DrawTexture (new Rect(pos.x-26, Screen.height - pos.y + 20, 52, 7), box);
		
		// draw health bar amount
		GUI.color = Color.green;
		GUI.DrawTexture (new Rect(pos.x-25, Screen.height - pos.y + 21, health/2, 5), box);	
	}
	/*
		// Combat
	public bool xxOnSerializeVars(NetworkWriter writer, int channelId, bool forceAll)
	{
		if (forceAll)
		{
			this.m_DirtyBits = 4294967295u;
		}

		bool dirty = false;

		if (channelId == 0)
		{
			if (!dirty) {
				writer.WritePackedUInt32(this.m_DirtyBits);
				dirty = true;
			}

			if ((this.m_DirtyBits & 1u) != 0u)
			{
				writer.WritePackedUInt32((uint)this.health);
			}
			if ((this.m_DirtyBits & 2u) != 0u)
			{
				writer.WritePackedUInt32((uint)this.otherInt1);
			}
			if ((this.m_DirtyBits & 4u) != 0u)
			{
				writer.WritePackedUInt32((uint)this.otherInt2);
			}
		}



		if (channelId == 1)
		{
			


			if ((this.m_DirtyBits & 8u) != 0u)
			{
				writer.Write(this.otherFloat);
			}
			if ((this.m_DirtyBits & 16u) != 0u)
			{
				writer.WritePackedUInt32((uint)this.otherInt3);
			}
		}

		if (channelId == 2)
		{
			Debug.Log("2");
		}
		
		if (channelId == 3)
		{
			Debug.Log("3");
		}

		if (channelId == 4)
		{
			Debug.Log("4");
		}

		if (!dirty) {
			writer.WritePackedUInt32(0);
		}
		return dirty;
	}

	*/
	/*
		// Combat
	public  bool yyyOnSerializeVars(NetworkWriter writer, int channelId, bool forceAll)
	{
		Debug.Log("OnSerializeVars Chan:" + channelId + " force:" + forceAll + " dirty:" + this.m_DirtyBits);

		if (forceAll)
		{
			this.m_DirtyBits = 4294967295u;
		}
		bool flag = false;
		if (channelId == 0 || forceAll)
		{
			if ((this.m_DirtyBits & 2u) != 0u)
			{
				if (!flag && !forceAll)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				Debug.Log("OnSerializeVars otherInt1:" + this.otherInt1);
				writer.WritePackedUInt32((uint)this.otherInt1);
			}
			if ((this.m_DirtyBits & 4u) != 0u)
			{
				if (!flag && !forceAll)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.otherInt2);
				Debug.Log("OnSerializeVars otherInt2:" + this.otherInt2);
			}
		}
		if (channelId == 1 || forceAll)
		{
			if ((this.m_DirtyBits & 8u) != 0u)
			{
				if (!flag && !forceAll)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.Write(this.otherFloat);
				Debug.Log("OnSerializeVars otherFloat:" + this.otherFloat);
			}
			if ((this.m_DirtyBits & 16u) != 0u)
			{
				if (!flag && !forceAll)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.otherInt3);
				Debug.Log("OnSerializeVars otherInt3:" + this.otherInt3);
			}
		}
		if (channelId == 2 || forceAll)
		{
			if ((this.m_DirtyBits & 1u) != 0u)
			{
				if (!flag && !forceAll)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.health);
				Debug.Log("OnSerializeVars health:" + this.health);
			}
		}
		if (!flag && !forceAll)
		{
			writer.WritePackedUInt32(this.m_DirtyBits);
		}
		return flag;
	}
	*/
	
	/*
		// Combat
	public  bool zzzOnSerializeVars(NetworkWriter writer, int channelId, bool forceAll)
	{
		//Debug.Log("OnSerializeVars Chan:" + channelId + " force:" + forceAll + " dirty:" + this.m_DirtyBits);

		if (forceAll)
		{
			// NOTE no dirty bits
			writer.WritePackedUInt32((uint)this.otherInt1);
			writer.WritePackedUInt32((uint)this.otherInt2);
			writer.Write(this.otherFloat);
			writer.WritePackedUInt32((uint)this.otherInt3);
			writer.WritePackedUInt32((uint)this.health);
			return true;
		}

		bool flag = false;
		if (channelId == 0)
		{
			if ((this.m_DirtyBits & 2u) != 0u)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.otherInt1);
			}
			if ((this.m_DirtyBits & 4u) != 0u)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.otherInt2);
			}
		}
		if (channelId == 1)
		{
			if ((this.m_DirtyBits & 8u) != 0u)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.Write(this.otherFloat);
			}
			if ((this.m_DirtyBits & 16u) != 0u)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.otherInt3);
			}

			if ((this.m_DirtyBits & 1u) != 0u)
			{
				if (!flag)
				{
					writer.WritePackedUInt32(this.m_DirtyBits);
					flag = true;
				}
				writer.WritePackedUInt32((uint)this.health);
			}
		}
		if (!flag)
		{
			writer.WritePackedUInt32(this.m_DirtyBits);
		}
		return flag;
	}
	




	// Combat
	public  void xxxxOnUnserializeVars(NetworkReader reader, int channelId, bool initialState)
	{
		if (initialState)
		{
			// NOTE no dirty bits
			this.health = (int)reader.ReadPackedUInt32();
			this.otherInt1 = (int)reader.ReadPackedUInt32();
			this.otherInt2 = (int)reader.ReadPackedUInt32();
			this.otherFloat = reader.ReadSingle();
			this.otherInt3 = (int)reader.ReadPackedUInt32();

			return;
		}

		uint num = reader.ReadPackedUInt32();

		//Debug.Log(" -- Combat OnUnserializeVars dirty:" + num + " ch:" + channelId);

		if (channelId == 0)
		{
			if ((num & 2) != 0)
			{
				this.otherInt1 = (int)reader.ReadPackedUInt32();
			}
			if ((num & 4) != 0)
			{
				this.otherInt2 = (int)reader.ReadPackedUInt32();
			}
		}

		if (channelId == 1)
		{
			if ((num & 8) != 0)
			{
				this.otherFloat = reader.ReadSingle();
			}
			if ((num & 16) != 0)
			{
				this.otherInt3 = (int)reader.ReadPackedUInt32();
			}
		}

		if (channelId == 3)
		{
			if ((num & 1) != 0)
			{
				this.OnDamage((int)reader.ReadPackedUInt32());
			}
		}
	}
	*/
}

