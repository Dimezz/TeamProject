/*
 * Jordan Rowe: 21/02/2015
 * 
 * The Enemy class contains information about enemies.
 */

using UnityEngine;
using System.Collections;

public class Asteroid : Spaceships2DObject
{
	private Camera mainCamera;
	private int points;

	void Start()
	{
		points = 1;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
	}

	void Update()
	{
		if (OutOfBounds())
		{
			Controller.RemoveSpaceship2DObject(gameObject, points);
		}
	}

	public override bool OutOfBounds()
	{
		Vector2 position = mainCamera.WorldToViewportPoint(transform.position);

		return (position.x > 1f || position.x < 0f || position.y > 1f || position.y < 0f);
	}

	public void Collision()
	{
		Controller.RemoveSpaceship2DObject(gameObject, -points);
	}	
}