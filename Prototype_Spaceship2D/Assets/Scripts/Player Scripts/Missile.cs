/*
 * Jordan Rowe: 27/02/2015
 * 
 * The Missile class contains information about missile speed and direction.
 */

using UnityEngine;
using System.Collections;

public class Missile : Spaceships2DObject 
{
	// The game object which shot this missile
	private IShooter parent;

	void Start()
	{
		mainCamera = GameObject.FindGameObjectWithTag ("MainCamera").camera;
		type = Spaceships2DObjectType.Missile;
	}

	void Update()
	{
		if (OutOfBounds())
		{
			parent.RemoveMissile(gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Asteroid" || collider.tag == "BasicEnemy")
		{
			collider.GetComponent<Asteroid>().HitByWeapon();
			parent.RemoveMissile(gameObject, true);
		}
	}

	// Missile position based off player location
	public void Fire(Transform startLocation, Direction direction, IShooter parentObject)
	{
		parent = parentObject;
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
            case Direction.UpRight:
                rigidbody2D.velocity = new Vector2(7.5f, 7.5f);
                transform.Rotate(new Vector3(0, 0, -45));
                break;
            case Direction.UpLeft:
                rigidbody2D.velocity = new Vector2(-7.5f, 7.5f);
                transform.Rotate(new Vector3(0, 0, 45));
                break;
            case Direction.DownRight:
                rigidbody2D.velocity = new Vector2(7.5f, -7.5f);
                transform.Rotate(new Vector3(0, 0, -135));
                break;
            case Direction.DownLeft:
                rigidbody2D.velocity = new Vector2(-7.5f, -7.5f);
                transform.Rotate(new Vector3(0, 0, 135));
                break;
		}
	}
}
