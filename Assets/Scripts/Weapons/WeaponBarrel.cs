using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class WeaponBarrel : MonoBehaviour
{
	[Header("Events")]
	public UnityEvent onShoot = new UnityEvent();

	public void Shoot()
	{
		onShoot.Invoke();
	}
}

