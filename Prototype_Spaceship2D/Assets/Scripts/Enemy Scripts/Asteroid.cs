/*
 * Jordan Rowe: 27/02/2015
 * 
 * The Enemy class contains information about enemies.
 */

using UnityEngine;
using System.Collections;

public class Asteroid : Spaceships2DObject
{
	private int pointsForAvoiding;
	private int pointsForDestroying;

	public int Damage
	{
		get; set;
	}

	void Start()
	{
		pointsForAvoiding = 1;
		pointsForDestroying = 10;
		pointsForCollision = -1;

		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		type = Spaceships2DObjectType.Asteroid;
	}

	void Update()
	{
		if (OutOfBounds())
		{
			Controller.RemoveSpaceship2DObject(gameObject, type, pointsForAvoiding);
		}
	}
	
	// More points if hit by a players missile
	public void HitByWeapon()
	{
		Controller.RemoveSpaceship2DObject(gameObject, type, pointsForDestroying);
	}
}