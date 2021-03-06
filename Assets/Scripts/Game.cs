﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using NaughtyAttributes;

public class Game : MonoBehaviour 
{
	public enum GameState : byte
	{
		WaitingForStart,
		Playing,
		Abandoning,
		Completed,
	}

	const string PlayerPrefabsPath = "Players";
	const float GizmosRadius = 0.3f;

	[SerializeField]
	[Tooltip("The duration of a game.")]
	private float gameDuration = 150f;

	[SerializeField]
	[Tooltip("The amount of time until a game is abandoned due to lack of players.")]
	private float abandonDuration = 10f;

	[SerializeField]
	[Tooltip("The amount of time until a finished game restarts.")]
	private float newGameDuration = 10f;

	[SerializeField, Required]
	[Tooltip("The Kid.")]
	private Kid kid;

	[Header("Spawning")]
	[SerializeField]
	[Tooltip("The spawn points used for players present at the start of the game.")]
	private Transform[] spawnPoints = new Transform[0];

	[SerializeField]
	[Tooltip("The spawn points used for players joining a game in play.")]
	private Transform[] dropInSpawnPoints = new Transform[0];

	[Header("Events")]
	public UnityEvent onStartGame = new UnityEvent();
	public UnityEvent onWinGame = new UnityEvent();
	public UnityEvent onLoseGame = new UnityEvent();
	public UnityEvent onStartAbandoningGame = new UnityEvent();
	public UnityEvent onAbandonGame = new UnityEvent();
	public UnityEvent onResumeAbandoningGame = new UnityEvent();
	public UnityEvent onEndGame = new UnityEvent();

	private Player[] playerPrefabs;
	private Dictionary<int, Player> players = new Dictionary<int, Player>();
	private GameState state = GameState.WaitingForStart;
	private float gameTimeRemaining;
	private float abandonTimeRemaining;
	private float newGameTimeRemaining;
	private string roomCode = string.Empty;

	public Kid Kid
	{
		get
		{
			return kid;
		}
	}

	public GameState State
	{
		get
		{
			return state;
		}
	}

	public float GameTimeRemaining
	{
		get
		{
			return gameTimeRemaining;
		}
	}

	public float AbandonTimeRemaining
	{
		get
		{
			return abandonTimeRemaining;
		}
	}

	public float NewGameTimeRemaining
	{
		get
		{
			return newGameTimeRemaining;
		}
	}

	public float Progress
	{
		get
		{
			return Mathf.InverseLerp(0f, gameDuration, gameTimeRemaining);
		}
	}

	public string RoomCode
	{
		get
		{
			return roomCode;
		}
	}

	private void Awake()
	{
		gameTimeRemaining = gameDuration;
		playerPrefabs = Resources.LoadAll<Player>(PlayerPrefabsPath);
		Toolbox.Game = this;
	}

	private void OnEnable()
	{
		if (AirConsole.instance)
		{
			AirConsole.instance.onConnect += HandleConnect;
			AirConsole.instance.onDisconnect += HandleDisconnect;
			AirConsole.instance.onReady += HandleReady;
		}
	}

	private void OnDisable()
	{
		if (AirConsole.instance)
		{
			AirConsole.instance.onConnect -= HandleConnect;
			AirConsole.instance.onDisconnect -= HandleDisconnect;
			AirConsole.instance.onReady -= HandleReady;
		}
	}
	private void Update()
	{
		#if UNITY_EDITOR
		// [1]: spawn an editor player with keyboard controls
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			AddNewPlayer(-1, true);
		}
		#endif

		// Game state
		if (state == GameState.Playing)
		{
			gameTimeRemaining -= Time.deltaTime;
			if (gameTimeRemaining < 0f)
			{
				WinGame();
			}
		}
		else if (state == GameState.Abandoning)
		{
			abandonTimeRemaining -= Time.deltaTime;
			if (abandonTimeRemaining < 0f)
			{
				LoseGame();
			}
		}
		else if (state == GameState.Completed)
		{
			newGameTimeRemaining -= Time.deltaTime;
			if (newGameTimeRemaining < 0f)
			{
				RestartGame();
			}
		}
	}

	private void HandleReady (string code)
	{
		Debug.LogFormat("Game Ready. Code={0}", code);
		roomCode = code;
		var connectedDevices = AirConsole.instance.GetControllerDeviceIds();
		foreach (int deviceId in connectedDevices) 
		{
			AddNewPlayer (deviceId, false);
		}
		UpdateAirConsoleGameState();
	}

	private void HandleConnect (int deviceId)
	{
		AddNewPlayer(deviceId, true);
		UpdateAirConsoleGameState();
	}

	private void AddNewPlayer(int deviceId, bool autoReady)
	{
		if (!players.ContainsKey(deviceId))
		{
			Debug.LogFormat("Added new player with id {0}", deviceId);
			// Spawn a random Player character at a random spawn point
			var prefab = playerPrefabs[Random.Range(0, playerPrefabs.Length)];
			var spawnPointList = (state == GameState.Playing || state == GameState.Abandoning) ? dropInSpawnPoints : spawnPoints;
			var spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Length)];
			var player = Instantiate<Player>(prefab, spawnPoint.position, spawnPoint.rotation);
			player.Initialize(deviceId, autoReady);
			players.Add(deviceId, player);

			if (state == GameState.WaitingForStart)
			{
				// Start game if it's waiting for players
				StartGame();
			}
			else if (state == GameState.Abandoning)
			{
				// Cancel a failing game if it's being abandoned
				ResumeAbandoningGame();
			}
		}
	}

	private void UpdateAirConsoleGameState()
	{
		// TODO: Update the gamestate for all of the players?
	}

	private void HandleDisconnect (int deviceId)
	{
		if (players.ContainsKey(deviceId))
		{
			// Notify the Player of their death and stop tracking them
			players[deviceId].Die();
			players.Remove(deviceId);
		}

		// If all players have left and the game is playing, lose the game after a timer
		var connectedDevices = AirConsole.instance.GetControllerDeviceIds();
		if (state == GameState.Playing && connectedDevices.Count == 0)
		{
			StartAbandoningGame();
		}
	}

	private void RestartGame()
	{
		// TODO: Restart game state and reset players as if they were using Ready
		Debug.LogError("Restarting Game (TODO)");
		kid.Health.Heal(Mathf.Infinity);

		foreach (var enemy in FindObjectsOfType<BoopTheTerror>())
		{
			Destroy(enemy.gameObject);
		}

		StartGame();
	}

	private void StartGame()
	{
		Debug.Log("Game Started");
		state = GameState.Playing;
		gameTimeRemaining = gameDuration;

		// Notify players of game start
		foreach (var pair in players)
		{
			pair.Value.Ready();
		}

		onStartGame.Invoke();
	}

	private void StartAbandoningGame()
	{
		Debug.Log("Starting to Abandon Game");
		state = GameState.Abandoning;
		abandonTimeRemaining = abandonDuration;
		onStartAbandoningGame.Invoke();
	}

	private void ResumeAbandoningGame()
	{
		Debug.Log("Resuming Abandoned Game");
		state = GameState.Playing;
		onResumeAbandoningGame.Invoke();
	}

	private void AbandonGame()
	{
		Debug.Log("Game Abandoned");
		onAbandonGame.Invoke();
		EndGame();
	}

	public void LoseGame()
	{
		Debug.Log("Game Lost");
		onLoseGame.Invoke();
		EndGame();
	}

	private void WinGame()
	{
		Debug.Log("Game Won");
		onWinGame.Invoke();
		EndGame();
	}

	private void EndGame()
	{
		Debug.Log("Game Over");
		state = GameState.Completed;
		newGameTimeRemaining = newGameDuration;
		onEndGame.Invoke();

		// Notify players of game end
		foreach (var pair in players)
		{
			pair.Value.Unready();
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

		foreach (var spawnPoint in dropInSpawnPoints)
		{
			Gizmos.color = Color.clear;
			Gizmos.DrawSphere(spawnPoint.position, GizmosRadius);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(spawnPoint.position, GizmosRadius);
		}
	}
}
