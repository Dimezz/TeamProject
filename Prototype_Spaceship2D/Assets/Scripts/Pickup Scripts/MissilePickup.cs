/*
 * Jordan Rowe: 27/02/2015
 * 
 * The MissilePickup class controls the missile pickups and their missile counts
 */

using UnityEngine;
using System.Collections;

public class MissilePickup : Spaceships2DObject 
{
	private int missileCount;	

	public int MissileCount
	{
		get { return missileCount; }
	}
	
	void Start()
	{
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		type = Spaceships2DObjectType.MissilePickup;

		// Randomized missile count
		missileCount = Random.Range(1, 5);
	}
	
	void Update()
	{
		if (OutOfBounds())
		{
			Controller.RemoveSpaceship2DObject(gameObject, type);
		}
	}
}
