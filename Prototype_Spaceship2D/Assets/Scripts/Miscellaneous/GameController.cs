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

public class GameController : MonoBehaviour
{
	public GameObject prefab;
	public GameObject healthPrefab;
	public int maxEnemies = 1;

	private GameObject currentHealthPickup;
	private float spawnRateTimer;
	private float minEnemySpeed;
	private float maxEnemySpeed;
	private int currentEnemies;
	private PlayerController player;
	private Camera mainCamera;
	private List<GameObject> enemyList;
	private bool inCoroutine;
	private int waveNumber;

	// User interface variables - health is stored in PlayerController
	private UserInterface userInterface;
	private int score;
	private float time;
	private string message;

	void Start()
	{
		enemyList = new List<GameObject>();
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		userInterface = GameObject.FindGameObjectWithTag("UI").GetComponent<UserInterface>();
		inCoroutine = false;
		currentEnemies = 0;
		minEnemySpeed = 3f;
		maxEnemySpeed = 5f;
		waveNumber = 1;

		// Init user interface
		score = 0;
		time = 0f;
		message = "";
	}

	void FixedUpdate()
	{
		time += Time.deltaTime;

		if (player.Health > 0)
		{
			ManageEnemies();
		}
		else
		{
			GameOver();
		}
		UpdateUserInterface();
	}

	// Removing object from the game
	public void RemoveSpaceship2DObject(GameObject shapeship2DObject, int points = 0)
	{
		enemyList.Remove(shapeship2DObject);
		Destroy(shapeship2DObject);
		currentEnemies--;
		score += points;
	}

	// Manage the number and speed of enemies
	void ManageEnemies()
	{
		if (enemyList.Count == 0 && !inCoroutine)
		{
			StartCoroutine(SpawnWave());
		}
	}

	// Controls the instantiation of enemies and pickups
	IEnumerator SpawnWave()
	{
		inCoroutine = true;
		bool pickupSpawned = false;
		message = "Wave: " + waveNumber.ToString() + " Enemies: " + maxEnemies.ToString();
		yield return new WaitForSeconds(1);
		int currentEnemiesAtStart = currentEnemies;

		for (; currentEnemiesAtStart < maxEnemies; currentEnemiesAtStart++)
		{
			if (player.Health <= 0)
			{
				break;
			}

			GameObject enemy = Instantiate(prefab) as GameObject;
			
			enemy.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
			enemy.GetComponent<Spaceships2DObject>().Controller = this;
			enemyList.Add(enemy);
			currentEnemies++;

			if (Random.Range(0, 50) == 1 && !pickupSpawned)
			{
				SpawnHealth();
				pickupSpawned = true;
			}

			yield return new WaitForSeconds(0.1f);
		}

		waveNumber++;
		maxEnemies += 5;

		if (minEnemySpeed < 4f)
		{
			minEnemySpeed += 0.2f;
		}
		if (maxEnemySpeed < 7f)
		{
			maxEnemySpeed += 0.2f;
		}
		inCoroutine = false;
	}

	void SpawnHealth()
	{
		currentHealthPickup = Instantiate(healthPrefab) as GameObject;

		currentHealthPickup.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
	}

	// Update the user interface
	void UpdateUserInterface()
	{
		userInterface.UpdateLives(player.Health);
		userInterface.UpdateScore(score);
		userInterface.UpdateTimer(time);
		userInterface.UpdateMessage(message);
	}

	void GameOver()
	{
		for (int index = enemyList.Count - 1; index >= 0; index--)
		{
			enemyList[index].GetComponent<Spaceships2DObject>().rigidbody2D.velocity = new Vector2(0, 0);
		}
		currentHealthPickup.rigidbody2D.velocity = new Vector2(0, 0);

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