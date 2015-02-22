using UnityEngine;
using System.Collections;

public class HealthPickup : Spaceships2DObject 
{
	public int health;
	
	private Camera mainCamera;

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

	public override bool OutOfBounds()
	{
		Vector2 position = mainCamera.WorldToViewportPoint(transform.position);
		
		return (position.x > 1f || position.x < 0f || position.y > 1f || position.y < 0f);
	}

	public override void Collision ()
	{
		Controller.RemoveSpaceship2DObject(gameObject, type);
	}
}
