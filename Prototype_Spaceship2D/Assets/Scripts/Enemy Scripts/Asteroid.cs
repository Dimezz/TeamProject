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

	public int Damage
	{
		get; set;
	}

	void Start()
	{
		points = 1;
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		type = Spaceships2DObjectType.Asteroid;
	}

	void Update()
	{
		if (OutOfBounds())
		{
			Controller.RemoveSpaceship2DObject(gameObject, type, points);
		}
	}

	public override bool OutOfBounds()
	{
		Vector2 position = mainCamera.WorldToViewportPoint(transform.position);

		return (position.x > 1f || position.x < 0f || position.y > 1f || position.y < 0f);
	}

	public override void Collision()
	{
		Controller.RemoveSpaceship2DObject(gameObject, type, -points);
	}	

	public void HitByWeapon()
	{
		Controller.RemoveSpaceship2DObject(gameObject, type, points * 10);
	}
}