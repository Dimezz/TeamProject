/*
 * Jordan Rowe: 27/02/2015
 * 
 * The PlayerController class controls the player movement and actions
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour, IShooter
{
	public float speed;
	public GameObject missilePrefab;
	public int missileCount = 5;
	private Vector2 velocity;

	// Sprites for the player
	public Sprite spaceshipUp;
	public Sprite spaceshipDown;
	public Sprite spaceshipLeft;
	public Sprite spaceshipRight;

	// Variables for keeping the player within the viewport
	private Camera mainCamera;
	private Vector3 size;
	private Vector2 positiveBoundary;
	private Vector2 negativeBoundary;

	// Game objects holding the sounds to be played
	private GameObject weaponFireSound;
	private GameObject weaponHitSound;

	// Missile list, timer and shoot rate
	private float weaponFireRate;
	private float weaponFireTimer;
	private List<GameObject> missileList;

	// Store the player direction - up, down, left, right
	private Direction direction;

	private int health;
	public int Health 
	{
		get { return health; }
	}

    private string shooterType;
    public string ShooterType
    {
        get { return shooterType; }
    }

	void Start()
	{
		// Get the size of the player
		size = new Vector3(renderer.bounds.size.x / 2.0f, renderer.bounds.size.y / 2.0f, 0);

		// Find game objects
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		weaponFireSound = GameObject.FindGameObjectWithTag("WeaponFire");
		weaponHitSound = GameObject.FindGameObjectWithTag("WeaponHit");

		// Setting up additional variables
		missileList = new List<GameObject>();
		health = 100;
		weaponFireRate = 1f;
		weaponFireTimer = 0f;
		direction = Direction.Up;

        shooterType = "Player";
	}

	void FixedUpdate()
	{
		// Update the weapon fire timer
		weaponFireTimer += Time.deltaTime;

		if (health > 0)
		{
			MovePlayer();
			FireMissiles();
		}
		else
		{
			// Game over
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

			// Don't let the health go negative
			if (health < 0)
			{
				health = 0;
			}
		}
		else if (collider.tag == "HealthPickup")
		{
			health += collider.GetComponent<HealthPickup>().health;

			// Health can't be greater than 100
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
        else if (collider.tag == "EnemyLaser")
        {
            health -= 10;

            if (health < 0)
            {
                health = 0;
            }
        }
        else if (collider.tag == "BasicEnemy")
        {
            collider.GetComponent<BasicEnemyController>().Collision();

            health -= 10;

            // Don't let the health go negative
            if (health < 0)
            {
                health = 0;
            }
        }
	}

	// Keep the playing inside the view port. WorldToViewportPoint normalizes the coordinates to be between 0.0 and 1.0
	void MovePlayer()
	{
		// Normalised position [0 - 1] of the player
		positiveBoundary = mainCamera.WorldToViewportPoint(transform.position + size);
		negativeBoundary = mainCamera.WorldToViewportPoint(transform.position - size);

		// Reset the player speed
		velocity = new Vector2(0, 0);

		// TODO: Change these from specific key pressed to input
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

	// Change which sprite is shown depending on the direction the player is facing
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

	// PEW PEW
	void FireMissiles()
	{
		// TODO: Change input from specific key code
		if (Input.GetKey(KeyCode.Space) && missileCount > 0 && weaponFireTimer > weaponFireRate)
		{
			weaponFireTimer = 0f;
			missileCount--;

			GameObject missile = Instantiate(missilePrefab) as GameObject;
			missile.GetComponent<Missile>().Fire(transform, direction, this);
			missileList.Add(missile);

			weaponFireSound.GetComponent<AudioSource>().Play();
		}
	}

	// If MonoDevelop gives you an error here change the target frame to 4.5 - Project -> Assembly-CSharp -> General
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