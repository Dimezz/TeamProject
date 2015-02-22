/*
 * Jordan Rowe: 21/02/2015
 * 
 * The Missile class contains information about missile speed and direction.
 */

using UnityEngine;
using System.Collections;

public class Missile : Spaceships2DObject 
{
	private Camera mainCamera;
	private GameObject player;

	void Start()
	{
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").camera;
		type = Spaceships2DObjectType.Missile;
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void Update()
	{
		if (OutOfBounds())
		{
			player.GetComponent<PlayerController>().RemoveMissile(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Asteroid")
		{
			collider.GetComponent<Asteroid>().HitByWeapon();
			player.GetComponent<PlayerController>().RemoveMissile(gameObject, true);
		}
	}

	public void Fire(Transform startLocation, Direction direction)
	{
		transform.position = startLocation.position;

		switch(direction)
		{
		case Direction.Up:
			rigidbody2D.velocity = new Vector2(0, 10);
			break;
		case Direction.Down:
			rigidbody2D.velocity = new Vector2(0, -10);
			transform.Rotate(new Vector3(0, 0, 180));
			break;
		case Direction.Left:
			rigidbody2D.velocity = new Vector2(-10, 0);
			transform.Rotate(new Vector3(0, 0, 90));
			break;
		case Direction.Right:
			rigidbody2D.velocity = new Vector2(10, 0);
			transform.Rotate(new Vector3(0, 0, -90));
			break;
		}
	}

	public override bool OutOfBounds()
	{
		Vector2 position = mainCamera.WorldToViewportPoint(transform.position);
		
		return (position.x > 1f || position.x < 0f || position.y > 1f || position.y < 0f);
	}
	
	public override void Collision () {}
}
