using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolboxLoader : MonoBehaviour 
{
	private void Awake()
	{
		Toolbox.Load();
		Destroy(gameObject);
	}
}
