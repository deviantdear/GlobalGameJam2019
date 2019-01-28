using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class AirConsoleJoystick : IAirConsoleInputReader
{
	private string id;
	private float returnSpeed;
	private Vector2 value = Vector2.zero;
	private bool pressed = false;

	public Vector2 Value
	{
		get
		{
			return value;
		}
	}

	public float X
	{
		get
		{
			return value.x;
		}
	}

	public float Y
	{
		get
		{
			return value.y;
		}
	}

	public AirConsoleJoystick(string id, float returnSpeed)
	{
		this.id = id;
		this.returnSpeed = returnSpeed;
	}

	#region IAirConsoleInputReader implementation
	public void Update(float deltaTime)
	{
		if (!pressed)
		{
			value = Vector2.Lerp(value, Vector2.zero, deltaTime);
		}
	}

	public void Process(JToken data)
	{
		var element = data[id];
		if (element != null)
		{
			var pressed = (bool?)data["pressed"];
			if (pressed.HasValue)
			{
				this.pressed = pressed.Value;
			}
			var message = element["message"];
			if (message != null)
			{
				value = new Vector2((float)message["x"], (float)message["y"]);
			}
		}
	}
	#endregion
	
}

