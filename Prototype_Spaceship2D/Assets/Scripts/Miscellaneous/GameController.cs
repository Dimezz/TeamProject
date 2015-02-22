/*
 * Jordan Rowe: 21/02/2015
 * 
 * The GameController class controls the instantiation of objects in the game
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Structure for holding an objects starting position and direction
public struct ObjectDirection
{
	public Vector2 start;
	public Direction direction;
	public float speed;
}

public enum Direction { Up = 1, Down, Left, Right};
public enum Spaceships2DObjectType { Asteroid = 1, HealthPickup, Missile, MissilePickup };

public class GameController : MonoBehaviour
{
	public GameObject asteroidPrefab;
	public GameObject healthPrefab;
	public GameObject missilePickupPrefab;
	public int maxEnemies = 1;

	private float spawnRateTimer;
	private float minEnemySpeed;
	private float maxEnemySpeed;
	private int currentEnemies;
	private PlayerController player;
	private Camera mainCamera;
	private List<GameObject> spaceships2DGameObjects;
	private bool inCoroutine;
	private int waveNumber;

	// User interface variables - health is stored in PlayerController
	private UserInterface userInterface;
	private int score;
	private float time;
	private string message;

	void Start()
	{
		spaceships2DGameObjects = new List<GameObject>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		userInterface = GameObject.FindGameObjectWithTag("UI").GetComponent<UserInterface>();
		inCoroutine = false;
		currentEnemies = 0;
		minEnemySpeed = 3f;
		maxEnemySpeed = 4f;
		waveNumber = 1;

		// Init user interface
		score = 0;
		time = 0f;
		message = "";
	}

	void FixedUpdate()
	{
		if (player.Health > 0)
		{
			time += Time.deltaTime;
			ManageEnemies();
		}
		else
		{
			GameOver();
		}
		UpdateUserInterface();

		print ("Current enemies: " + currentEnemies.ToString() + " Total game objects: " + spaceships2DGameObjects.Count.ToString());
	}

	// Removing object from the game
	public void RemoveSpaceship2DObject(GameObject shapeship2DObject, Spaceships2DObjectType type, int points = 0)
	{
		if (type == Spaceships2DObjectType.Asteroid)
		{
			currentEnemies--;
		}

		spaceships2DGameObjects.Remove(shapeship2DObject);
		Destroy(shapeship2DObject);
		score += points;
	}

	// Manage the number and speed of enemies
	void ManageEnemies()
	{
		if (currentEnemies == 0 && !inCoroutine)
		{
			StartCoroutine(SpawnStandardWave());
		}
	}

	// Controls the instantiation of enemies and pickups
	IEnumerator SpawnStandardWave()
	{
		inCoroutine = true;

		if (waveNumber % 5 == 0)
		{
			message = "Boss Wave: " + waveNumber.ToString() + " Enemies: " + maxEnemies.ToString();
		}
		else
		{
			message = "Wave: " + waveNumber.ToString() + " Enemies: " + maxEnemies.ToString();
		}

		yield return new WaitForSeconds(2);
		int currentEnemiesAtStart = currentEnemies;

		for (; currentEnemiesAtStart < maxEnemies; currentEnemiesAtStart++)
		{
			if (player.Health <= 0)
			{
				break;
			}

			if (waveNumber % 5 == 0)
			{
				SpawnAsteroid(1.5f);
			}
			else
			{
				SpawnAsteroid();
			}

			if (Random.Range(0, 50) == 1)
			{
				SpawnHealth();
			}

			if (Random.Range(0, 50) == 2)
			{
				SpawnMissile();
			}

			yield return new WaitForSeconds(0.1f);
		}

		waveNumber++;
		maxEnemies += 5;

		if (minEnemySpeed < 4f)
		{
			minEnemySpeed += 0.2f;
		}
		if (maxEnemySpeed < 6f)
		{
			maxEnemySpeed += 0.2f;
		}
		inCoroutine = false;
	}

	void SpawnHealth()
	{
		GameObject healthPickup = Instantiate(healthPrefab) as GameObject;

		healthPickup.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
		healthPickup.GetComponent<Spaceships2DObject>().Controller = this;
		spaceships2DGameObjects.Add(healthPickup);
	}

	void SpawnAsteroid(float scale = 1)
	{
		GameObject enemy = Instantiate(asteroidPrefab) as GameObject;
		
		enemy.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
		enemy.GetComponent<Spaceships2DObject>().Controller = this;
		enemy.GetComponent<Asteroid>().Damage = (int)(10f * scale);
		enemy.transform.localScale *= scale;
		spaceships2DGameObjects.Add(enemy);
		currentEnemies++;
	}

	void SpawnMissile()
	{
		GameObject missilePickup = Instantiate(missilePickupPrefab) as GameObject;
		
		missilePickup.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
		missilePickup.GetComponent<Spaceships2DObject>().Controller = this;
		spaceships2DGameObjects.Add(missilePickup);
	}

	// Update the user interface
	void UpdateUserInterface()
	{
		userInterface.UpdateLives(player.Health);
		userInterface.UpdateScore(score);
		userInterface.UpdateTimer(time);
		userInterface.UpdateMessage(message);
		userInterface.UpdateMissileCount(player.missileCount);
	}

	void GameOver()
	{
		for (int index = spaceships2DGameObjects.Count - 1; index >= 0; index--)
		{
			spaceships2DGameObjects[index].GetComponent<Spaceships2DObject>().rigidbody2D.velocity = new Vector2(0, 0);
		}
		message = "Game Over! Press R to respawn.";
	}

	// Create a new ObjectDirection with the enemy starting location and direction
	ObjectDirection RandomSpawnLocation()
	{
		ObjectDirection objectDirection = new ObjectDirection();

		objectDirection.direction = (Direction)Random.Range(1, 4);
		objectDirection.speed = Random.Range(minEnemySpeed, maxEnemySpeed);

		switch (objectDirection.direction)
		{
		case Direction.Up:
			objectDirection.start = new Vector2(Random.Range(0f, 1f), 0f);
			break;
		case Direction.Down:
			objectDirection.start = new Vector2(Random.Range(0f, 1f), 1f);
			break;
		case Direction.Left:
			objectDirection.start = new Vector2(1f, Random.Range(0f, 1f));
			break;
		case Direction.Right:
			objectDirection.start = new Vector2(0f, Random.Range(0f, 1f));
			break;
		}

		objectDirection.start = mainCamera.ViewportToWorldPoint(objectDirection.start);
		
		return objectDirection;
	}
}