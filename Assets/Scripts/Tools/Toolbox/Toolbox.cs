// From http://wiki.unity3d.com/index.php/Toolbox
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Toolbox : Singleton<Toolbox> 
{
	/*
	How to add something to Toolbox:

	[SerializeField] private Foo foo;
	public static Foo Foo { get { return Instance.foo; } }
	*/

	[SerializeField]
	private AirConsoleInput input;

	public static AirConsoleInput Input
	{
		get
		{
			return Instance.input;
		}
	}

	protected Toolbox () {} // guarantee this will be always a singleton only
}
