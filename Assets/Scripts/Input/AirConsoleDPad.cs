using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class AirConsoleDPad : IAirConsoleInputReader
{
	private string id;
	private bool up = false;
	private bool down = false;
	private bool left = false;
	private bool right = false;

	public Vector2 Value
	{
		get
		{
			return new Vector2(X, Y);
		}
	}

	public float X
	{
		get
		{
			return (left ? -1f : 0f) + (right ? 1f : 0f);
		}
	}

	public float Y
	{
		get
		{
			return (up ? 1f : 0f) + (down ? -1f : 0f);
		}
	}

	public AirConsoleDPad(string id)
	{
		this.id = id;
	}

	#region IAirConsoleInputReader implementation
	public void Update(float deltaTime) { }

	public void Process(JToken data)
	{
		if (data["element"].ToString().Equals(id))
		{
			var key = (string)data["data"]["key"];
			var pressed = (bool?)data["data"]["pressed"];
			if (pressed.HasValue)
			{
				switch (key)
				{
					case "up":
						up = pressed.Value;
						break;
					case "down":
						down = pressed.Value;
						break;
					case "left":
						left = pressed.Value;
						break;
					case "right":
						right = pressed.Value;
						break;
				}
			}
		}
	}
	#endregion
	
}

