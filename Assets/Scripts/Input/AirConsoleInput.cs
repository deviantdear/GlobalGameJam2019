using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class AirConsoleInput : MonoBehaviour
{
	private Dictionary<int, AirConsoleController> inputs = new Dictionary<int, AirConsoleController>();

	private void OnEnable()
	{
		if (AirConsole.instance)
		{
			AirConsole.instance.onMessage += HandleMessage;
		}
		else
		{
			Debug.LogWarning("No AirConsole instance was found! AirConsoleInput is unable to start up.");
		}
	}

	private void OnDisable()
	{
		if (AirConsole.instance)
		{
			AirConsole.instance.onMessage -= HandleMessage;
		}
	}

	private void LateUpdate()
	{
		foreach (var pair in inputs)
		{
			pair.Value.Update(Time.deltaTime);
		}
	}

	private void HandleMessage(int from, JToken data)
	{
		if (!inputs.ContainsKey(from))
		{
			inputs.Add(from, new AirConsoleController());
		}
		inputs[from].Process(data);
	}

	public bool HasController(int deviceId)
	{
		return inputs.ContainsKey(deviceId);
	}

	public AirConsoleController GetController(int deviceId)
	{
		if (HasController(deviceId))
			return inputs[deviceId];
		return null;
	}
}

