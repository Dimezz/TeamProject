/*
 * Jordan Rowe: 21/02/2015
 * 
 * The PlayerController class controls the player movement and actions
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour 
{
	public float speed;
	public GameObject missilePrefab;
	public Sprite spaceshipUp;
	public Sprite spaceshipDown;
	public Sprite spaceshipLeft;
	public Sprite spaceshipRight;
	public int missileCount = 5;

	private Camera mainCamera;
	private Vector3 size;
	private Vector2 velocity;
	private Vector2 positiveBoundary;
	private Vector2 negativeBoundary;

	private GameObject weaponFireSound;
	private GameObject weaponHitSound;
	private float weaponFireRate;
	private float weaponFireTimer;
	private List<GameObject> missileList;

	private Direction direction;

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
		weaponFireSound = GameObject.FindGameObjectWithTag("WeaponFire");
		weaponHitSound = GameObject.FindGameObjectWithTag("WeaponHit");
		missileList = new List<GameObject>();
		health = 100;
		weaponFireRate = 1f;
		weaponFireTimer = 0f;
		direction = Direction.Up;
	}

	void FixedUpdate()
	{
		weaponFireTimer += Time.deltaTime;

		if (health > 0)
		{
			MovePlayer();
			FireMissiles();
		}
		else
		{
			rigidbody2D.velocity = new Vector2(0, 0);

			if (Input.GetKey(KeyCode.R))
			{
				Application.LoadLevel(Application.loadedLevelName);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Asteroid")
		{
			collider.GetComponent<Asteroid>().Collision();

			GetComponent<AudioSource>().Play();

			health -= collider.GetComponent<Asteroid>().Damage;

			if (health < 0)
			{
				health = 0;
			}
		}
		else if (collider.tag == "HealthPickup")
		{
			health += collider.GetComponent<HealthPickup>().health;

			if (health > 100)
			{
				health = 100;
			}

			collider.GetComponent<HealthPickup>().Collision();
		}
		else if (collider.tag == "MissilePickup")
		{
			missileCount += collider.GetComponent<MissilePickup>().MissileCount;
			collider.GetComponent<MissilePickup>().Collision();
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
			direction = Direction.Right;
			velocity = new Vector2(speed, 0);
		}
		else if (Input.GetKey(KeyCode.A) && negativeBoundary.x > 0f)
		{
			direction = Direction.Left;
			velocity = new Vector2(-speed, 0);
		}
		else if (Input.GetKey(KeyCode.W) && positiveBoundary.y < 1f)
		{
			direction = Direction.Up;
			velocity = new Vector2(velocity.x, speed);
		}
		else if (Input.GetKey(KeyCode.S) && negativeBoundary.y > 0f)
		{
			direction = Direction.Down;
			velocity = new Vector2(velocity.x, -speed);
		}
		
		rigidbody2D.velocity = velocity;

		ChangeSprite();
	}

	void ChangeSprite()
	{
		switch (direction)
		{
		case Direction.Up:
			GetComponent<SpriteRenderer>().sprite = spaceshipUp;
			break;
		case Direction.Down:
			GetComponent<SpriteRenderer>().sprite = spaceshipDown;
			break;
		case Direction.Left:
			GetComponent<SpriteRenderer>().sprite = spaceshipLeft;
			break;
		case Direction.Right:
			GetComponent<SpriteRenderer>().sprite = spaceshipRight;
			break;
		}
	}

	void FireMissiles()
	{
		if (Input.GetKey(KeyCode.Space) && missileCount > 0 && weaponFireTimer > weaponFireRate)
		{
			weaponFireTimer = 0f;
			missileCount--;

			GameObject missile = Instantiate(missilePrefab) as GameObject;
			missile.GetComponent<Missile>().Fire(transform, direction);
			missileList.Add(missile);

			weaponFireSound.GetComponent<AudioSource>().Play();
		}
	}

	public void RemoveMissile(GameObject missile, bool hitObject = false)
	{
		missileList.Remove(missile);
		Destroy(missile);

		if (hitObject)
		{
			weaponHitSound.GetComponent<AudioSource>().Play();
		}
	}
}