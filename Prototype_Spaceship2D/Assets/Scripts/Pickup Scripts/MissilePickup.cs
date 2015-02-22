using UnityEngine;
using System.Collections;

public class MissilePickup : Spaceships2DObject 
{
	private int missileCount;	
	private Camera mainCamera;

	public int MissileCount
	{
		get { return missileCount; }
	}
	
	void Start()
	{
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		type = Spaceships2DObjectType.MissilePickup;
		missileCount = Random.Range(1, 5);
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
