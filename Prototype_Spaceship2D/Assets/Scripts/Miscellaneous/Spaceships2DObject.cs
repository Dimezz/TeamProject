/*
 * Jordan Rowe: 21/02/2015
 * 
 * The Spaceships2DObject class contains information about all game objects.
 */

using UnityEngine;
using System.Collections;

public abstract class Spaceships2DObject : MonoBehaviour
{
	public GameController Controller { get; set; }

	public abstract bool OutOfBounds();

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