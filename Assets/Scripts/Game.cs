using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Game : MonoBehaviour 
{
	private enum State : byte
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
	private State state = State.WaitingForStart;
	private float gameTimeRemaining;
	private float abandonTimeRemaining;
	private float newGameTimeRemaining;

	public float Progress
	{
		get
		{
			return Mathf.InverseLerp(0f, gameDuration, gameTimeRemaining);
		}
	}

	private void Awake()
	{
		playerPrefabs = Resources.LoadAll<Player>(PlayerPrefabsPath);
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
			AddNewPlayer(-1);
		}
		#endif

		// Game state
		if (state == State.Playing)
		{
			gameTimeRemaining -= Time.deltaTime;
			if (gameTimeRemaining < 0f)
			{
				WinGame();
			}
		}
		else if (state == State.Abandoning)
		{
			abandonTimeRemaining -= Time.deltaTime;
			if (abandonTimeRemaining < 0f)
			{
				LoseGame();
			}
		}
		else if (state == State.Completed)
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
		Debug.Log("Game Ready");
		List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
		foreach (int deviceId in connectedDevices) 
		{
			AddNewPlayer (deviceId);
		}
	}

	private void HandleConnect (int deviceId)
	{
		AddNewPlayer(deviceId);
	}

	private void AddNewPlayer(int deviceId)
	{
		if (!players.ContainsKey(deviceId))
		{
			Debug.LogFormat("Added new player with id {0}", deviceId);
			// Spawn a random Player character at a random spawn point
			var prefab = playerPrefabs[Random.Range(0, playerPrefabs.Length)];
			var spawnPointList = (state == State.Playing) ? dropInSpawnPoints : spawnPoints;
			var spawnPoint = spawnPointList[Random.Range(0, spawnPointList.Length)];
			var player = Instantiate<Player>(prefab, spawnPoint.position, spawnPoint.rotation);
			player.Initialize(deviceId, state == State.Playing);
			players.Add(deviceId, player);

			if (state == State.WaitingForStart)
			{
				// Start game if it's waiting for players
				StartGame();
			}
			else if (state == State.Abandoning)
			{
				// Cancel a failing game if it's being abandoned
				ResumeAbandoningGame();
			}
		}
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
		if (state == State.Playing)
		{
			StartAbandoningGame();
		}
	}

	private void RestartGame()
	{
		// TODO: Restart game state and reset players as if they were using Ready
		Debug.LogError("Restarting Game (TODO)");
	}

	private void StartGame()
	{
		Debug.Log("Game Started");
		state = State.Playing;
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
		state = State.Abandoning;
		abandonTimeRemaining = abandonDuration;
		onStartAbandoningGame.Invoke();
	}

	private void ResumeAbandoningGame()
	{
		Debug.Log("Resuming Abandoned Game");
		state = State.Playing;
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
		state = State.Completed;
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
