using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public class AirConsoleController : IAirConsoleInputReader
{
	private AirConsoleDPad move = new AirConsoleDPad("dpad-left");
	private AirConsoleDPad aim = new AirConsoleDPad("dpad-right");

	public AirConsoleDPad Move
	{
		get
		{
			return move;
		}
	}

	public AirConsoleDPad Aim
	{
		get
		{
			return aim;
		}
	}

	#region IAirConsoleInputReaderElement implementation
	public void Update(float deltaTime)
	{
		move.Update(deltaTime);
		aim.Update(deltaTime);
	}

	public void Process(JToken data)
	{
		move.Process(data);
		aim.Process(data);
	}
	#endregion
	
}

