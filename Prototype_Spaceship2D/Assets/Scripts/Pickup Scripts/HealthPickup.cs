/*
 * Jordan Rowe: 27/02/2015
 * 
 * The HealthPickup class controls the health pickups
 */

using UnityEngine;
using System.Collections;

public class HealthPickup : Spaceships2DObject 
{
	public int health;

	void Start()
	{
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		type = Spaceships2DObjectType.HealthPickup;
	}

	void Update()
	{
		if (OutOfBounds())
		{
			Controller.RemoveSpaceship2DObject(gameObject, type);
		}
	}
}
