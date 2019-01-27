using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;

public class PlayerSpawner : MonoBehaviour
{
	const string PlayerPrefabsPath = "Players";
	const float GizmosRadius = 0.3f;

	[SerializeField]
	private Transform[] spawnPoints;

	private Player[] playerPrefabs;

	private Dictionary<int, Player> players = new Dictionary<int, Player>();

	private void Awake()
	{
		playerPrefabs = Resources.LoadAll<Player>(PlayerPrefabsPath);
	}

	private void Start()
	{
		if (AirConsole.instance)
		{
			AirConsole.instance.onConnect += HandleDeviceConnect;
			AirConsole.instance.onDisconnect += HandleDeviceDisconnect;
		}
	}

	#if UNITY_EDITOR
	private void Update()
	{
		// [1]: spawn an editor player with keyboard controls
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			HandleDeviceConnect(-1);
		}
	}
	#endif

	private void HandleDeviceConnect (int deviceId)
	{
		if (!players.ContainsKey(deviceId))
		{
			// Spawn a random Player character at a random Spawn Point
			var prefab = playerPrefabs[Random.Range(0, playerPrefabs.Length)];
			var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
			var player = Instantiate<Player>(prefab, spawnPoint.position, spawnPoint.rotation);
			player.Initialize(deviceId);
			players.Add(deviceId, player);
		}
	}

	private void HandleDeviceDisconnect (int deviceId)
	{
		if (players.ContainsKey(deviceId))
		{
			// Notify the Player of their death and stop tracking them
			players[deviceId].Die();
			players.Remove(deviceId);
		}
	}

	private void OnDestroy()
	{
		if (AirConsole.instance)
		{
			AirConsole.instance.onDisconnect -= HandleDeviceConnect;
			AirConsole.instance.onDisconnect -= HandleDeviceDisconnect;
		}
	}

	private void OnDrawGizmos()
	{
		foreach (var spawnPoint in spawnPoints)
		{
			Gizmos.color = Color.clear;
			Gizmos.DrawSphere(spawnPoint.position, GizmosRadius);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(spawnPoint.position, GizmosRadius);
		}
	}
}

