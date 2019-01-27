using UnityEngine;
using System.Collections;
using NaughtyAttributes;

public class Kid : MonoBehaviour
{
	[SerializeField, Required]
	[Tooltip("The kid's health.")]
	private Health health;

	public Health Health
	{
		get
		{
			return health;
		}
	}

	[Button("Find Health")]
	private void FindHealth()
	{
		var foundHealth = GetComponentInChildren<Health>();
		if (foundHealth != health)
		{
			health = foundHealth;
			#if UNITY_EDITOR
			UnityEditor.EditorUtility.SetDirty(this);
			#endif
		}
	}

	private void Awake()
	{
		health.onDie.AddListener(HandleDie);
	}

	private void HandleDie()
	{
		var game = FindObjectOfType<Game>();
		if (game)
		{
			game.LoseGame();
		}
	}
}

