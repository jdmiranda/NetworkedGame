using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class SpaceManager : NetworkManager 
{
	NetworkMatch networkMatch;
	bool matchCreated;

	public void Start()
	{
		networkMatch = gameObject.AddComponent<NetworkMatch>();
		//chech for batchmode
		if(SystemInfo.graphicsDeviceID == 0)
		{
			CreateMatch();
		}
	}

	public void CreateMatch()
	{
		UnityEngine.Debug.Log("In BatchMode, starting game server!");
		CreateMatchRequest create = new CreateMatchRequest();
		create.name = "Default";
		create.size = 12;
		create.advertise = true;
		create.password = "";
		
		networkMatch.CreateMatch(create, OnMatchCreate);
	}

	public void OnMatchCreate(CreateMatchResponse matchResponse)
	{
		if (matchResponse.success)
		{
			UnityEngine.Debug.Log("Create match succeeded");
			matchCreated = true;
			Utility.SetAccessTokenForNetwork(matchResponse.networkId, new NetworkAccessToken(matchResponse.accessTokenString));
			StartGameServer(new MatchInfo(matchResponse));
//			NetworkServer.Listen(new MatchInfo(matchResponse), 9000);
		}
		else
		{
			UnityEngine.Debug.LogError ("Create match failed");
		}
	}

	public void JoinMatchGame()
	{
		networkMatch.ListMatches(0, 20, "", OnMatchList);
	}

	public void OnMatchList(ListMatchResponse matchListResponse)
	{
		if (matchListResponse.success && matchListResponse.matches != null)
		{
			networkMatch.JoinMatch(matchListResponse.matches[0].networkId, "", OnMatchJoined);
		}
	}

	public void OnMatchJoined(JoinMatchResponse matchJoin)
	{
		if (matchJoin.success)
		{
			UnityEngine.Debug.Log("Join match succeeded");
			if (matchCreated)
			{
				UnityEngine.Debug.LogWarning("Match already set up, aborting...");
				return;
			}

			NetworkManager.singleton.StartClient(new MatchInfo(matchJoin));
//			Utility.SetAccessTokenForNetwork(matchJoin.networkId, new NetworkAccessToken(matchJoin.accessTokenString));
//			NetworkClient myClient = new NetworkClient();
//			myClient.RegisterHandler(MsgType.Connect, OnConnected);
//			myClient.Connect(new MatchInfo(matchJoin));
		}
		else
		{
			UnityEngine.Debug.LogError("Join match failed");
		}
	}

//	static SpaceManager single;
//
//	Stopwatch watch;
//	float last;
//
//	List<float> rtts;
//
//	void Start()
//	{
//		single = this;
//		watch = new Stopwatch();
//		watch.Start();
//		rtts = new List<float>();
//	}
//
//	void OnGUI()
//	{
//		GUI.Label(new Rect(300,10,200,20), "RTT:"+last+"ms");
//
//		if (!NetworkClient.active)
//			return;
//
//		int posY = 30;
//		foreach (var rtt in rtts)
//		{
//			int n = (int)(rtt / 5);
//			if (n > 20) n = 20;
//
//			string s = "";
//			for (int i=0; i < n; i++)
//			{
//				s = s + ".";
//			}
//			GUI.Label(new Rect(340,10+posY,200,20), s);
//			posY += 8;
//		}
//
//	}
//
//	static public void Reset()
//	{
//		single.watch.Reset();
//		single.watch.Start();
//	}
//	static public float Now()
//	{
//		single.last = single.watch.ElapsedMilliseconds;
//
//		single.rtts.Add(single.last);
//		if (single.rtts.Count > 20)
//			single.rtts.RemoveAt(0);
//
//		return single.last;
//	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		Vector3 spawnPos = Vector3.right * conn.connectionId;
		GameObject player = (GameObject)Instantiate(base.playerPrefab, spawnPos, Quaternion.identity);
 		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

	public void StartGameServer(MatchInfo matchInfo)
	{
		UnityEngine.Debug.Log("Starting match game server");
//		NetworkManager.singleton.StartServer(matchInfo);
		NetworkManager.singleton.StartHost(matchInfo);
	}

	public void StartGame()
	{
		NetworkManager.singleton.StartHost();
	}

	public void JoinGame()
	{
		NetworkManager.singleton.StartClient();
	}
}
