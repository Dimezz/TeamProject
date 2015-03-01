/*
 * Jordan Rowe: 27/02/2015
 * 
 * The Spaceships2DObject class contains information about all game objects
 */

using UnityEngine;
using System.Collections;

// Structure for holding an objects starting position and direction
public struct ObjectDirection
{
	public Vector2 start;
	public Direction direction;
	public float speed;
}

public enum Direction { Up = 1, Down, Left, Right, UpRight, UpLeft, DownRight, DownLeft };
public enum Spaceships2DObjectType { Asteroid = 1, HealthPickup, Missile, MissilePickup, BasicEnemy, Player };

public abstract class Spaceships2DObject : MonoBehaviour
{
	protected Camera mainCamera;
	protected int pointsForCollision;

	// Also game objects have a specific type
	protected Spaceships2DObjectType type;

    public Spaceships2DObjectType Type
    {
        get { return type;  }
    }

	// May be useful the set the game controller. Should be set to null if not used
	public GameController Controller { get; set; }
	
	public virtual void Collision()
	{
		Controller.RemoveSpaceship2DObject(gameObject, type, pointsForCollision);
	}

	// Check if outside the viewport - should work for most objects
	public virtual bool OutOfBounds()
	{
		Vector2 position = mainCamera.WorldToViewportPoint(transform.position);
		
		return (position.x > 1f || position.x < 0f || position.y > 1f || position.y < 0f);
	}

	// Basic game object instantiation for up, down, left and right direction
	public virtual void Instantiate(ObjectDirection objectDirection)
	{
		transform.position = objectDirection.start;
		
		switch(objectDirection.direction)
		{
		case Direction.Up:
			rigidbody2D.velocity = new Vector2(0, 1) * objectDirection.speed;
			break;
		case Direction.Down:
			rigidbody2D.velocity = new Vector2(0, -1) * objectDirection.speed;
			break;
		case Direction.Left:
			rigidbody2D.velocity = new Vector2(-1, 0) * objectDirection.speed;
			break;
		case Direction.Right:
			rigidbody2D.velocity = new Vector2(1, 0) * objectDirection.speed;
			break;
		}
	}
}