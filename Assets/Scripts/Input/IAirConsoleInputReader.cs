using UnityEngine;
using System.Collections;
using Newtonsoft.Json.Linq;

public interface IAirConsoleInputReader
{
	void Update(float deltaTime);

	void Process(JToken data);
}

