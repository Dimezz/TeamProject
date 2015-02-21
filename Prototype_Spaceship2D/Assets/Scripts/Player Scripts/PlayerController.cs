/*
 * Jordan Rowe: 21/02/2015
 * 
 * The PlayerController class controls the player movement and actions
 */

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	public float speed;

	private Camera mainCamera;
	private Vector3 size;
	private Vector2 velocity;
	private Vector2 positiveBoundary;
	private Vector2 negativeBoundary;

	private int health;
	public int Health 
	{
		get { return health; }
	}

	void Start()
	{
		// Get the size of the player
		size = new Vector3((float)(renderer.bounds.size.x / 2.0), (float)(renderer.bounds.size.y / 2.0), 0);
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		health = 100;
	}

	void FixedUpdate()
	{
		if (health > 0)
		{
			MovePlayer();
		}
		else if (Input.GetKey(KeyCode.R))
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Asteroid")
		{
			collider.GetComponent<Asteroid>().Collision();

			GetComponent<AudioSource>().Play();

			rigidbody2D.velocity = new Vector2(0, 0);
			health -= 10;
		}
		else if (collider.tag == "HealthPickup")
		{
			health += collider.gameObject.GetComponent<HealthPickup>().health;

			if (health > 100)
			{
				health = 100;
			}

			Destroy(collider.gameObject);
		}
	}

	// Keep the playing inside the view port. WorldToViewportPoint normalizes the coordinates to be between 0.0 and 1.0
	void MovePlayer()
	{
		positiveBoundary = mainCamera.WorldToViewportPoint(transform.position + size);
		negativeBoundary = mainCamera.WorldToViewportPoint(transform.position - size);
		
		velocity = new Vector2(0, 0);
		
		if (Input.GetKey(KeyCode.D) && positiveBoundary.x < 1f)
		{
			velocity = new Vector2(speed, 0);
		}
		else if (Input.GetKey(KeyCode.A) && negativeBoundary.x > 0f)
		{
			velocity = new Vector2(-speed, 0);
		}
		
		if (Input.GetKey(KeyCode.W) && positiveBoundary.y < 1f)
		{
			transform.localScale = new Vector3(1, 1, 1);
			velocity = new Vector2(velocity.x, speed);
		}
		else if (Input.GetKey(KeyCode.S) && negativeBoundary.y > 0f)
		{
			transform.localScale = new Vector3(1, -1, 1);
			velocity = new Vector2(velocity.x, -speed);
		}
		
		rigidbody2D.velocity = velocity;
	}
}