using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Events;

public class Lifespan : MonoBehaviour 
{
	[SerializeField]
	[Tooltip("The number of seconds the object will exist before being destroyed.")]
	[MinValue(0f)]
	private float lifespan = 10f;

	[SerializeField]
	[Tooltip("Should the lifespan start ticking automatically on start?")]
	private bool beginOnStart = true;

	[Header("Events")]
	public UnityEvent onDestroy = new UnityEvent();

	private void Start()
	{
		if (beginOnStart)
		{
			Begin();
		}
	}

	[ContextMenu("Begin")]
	public void Begin()
	{
		Invoke("DestroyDelayed", lifespan);
	}

	[ContextMenu("Cancel")]
	public void Cancel()
	{
		CancelInvoke("DestroyDelayed");
	}

	[ContextMenu("Restart")]
	public void Restart()
	{
		Cancel();
		Begin();
	}

	private void DestroyDelayed()
	{
		onDestroy.Invoke();
		Destroy(gameObject);
	}
}
