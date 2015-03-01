/*
 * Jordan Rowe: 27/02/2015
 * 
 * The GameController class controls the instantiation of objects in the game
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
	public GameObject asteroidPrefab;
	public GameObject healthPrefab;
	public GameObject missilePickupPrefab;
    public GameObject basicEnemyPrefab;
	public int maxEnemies = 1;

	// Will need to access the player control
	private PlayerController player;

	// Variables to control the enemies/asteroids
	private float spawnRateTimer;
	private float minEnemySpeed;
	private float maxEnemySpeed;
	private int currentEnemies;
    private int maxBossEnemies = 5;

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
		// Create new list of game objects
		spaceships2DGameObjects = new List<GameObject>();

		// Find game objects
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		userInterface = GameObject.FindGameObjectWithTag("UI").GetComponent<UserInterface>();

		// Additional variables
		inCoroutine = false;
		currentEnemies = 0;
		minEnemySpeed = 3f;
		maxEnemySpeed = 4f;
		waveNumber = 1;

		// Initialise user interface
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

		// DEBUG
		print ("Current enemies: " + currentEnemies.ToString() + " Total game objects: " + spaceships2DGameObjects.Count.ToString());
	}

	// Removing object from the game
	public void RemoveSpaceship2DObject(GameObject shapeship2DObject, Spaceships2DObjectType type, int points = 0)
	{
		if (type == Spaceships2DObjectType.Asteroid || type == Spaceships2DObjectType.BasicEnemy)
		{
			currentEnemies--;
		}

		// Remove the game object from the list THEN destroy it
		spaceships2DGameObjects.Remove(shapeship2DObject);
		Destroy(shapeship2DObject);
		score += points;
	}

	// Manage the number and speed of enemies
	void ManageEnemies()
	{
		// Only spawn a wave when there are no enemies and are not already in the coroutine
		if (currentEnemies == 0 && !inCoroutine)
        {
            if (waveNumber % 5 == 0)
            {
                StartCoroutine(SpawnBossWave());
            }
            else
            {
                StartCoroutine(SpawnStandardWave());
            }
		}
	}

	// Controls the instantiation of asteriods and pickups
	IEnumerator SpawnStandardWave()
	{
        inCoroutine = true;
        UpdateUIMessage();


		yield return new WaitForSeconds(2);
		for (int i = 0; i < maxEnemies; i++, currentEnemies++)
		{
			if (player.Health <= 0)
			{
				break;
			}

            SpawnAsteroid();
			SpawnHealth();
			SpawnMissile();

			yield return new WaitForSeconds(0.1f);
		}

		if (minEnemySpeed < 4f)
		{
			minEnemySpeed += 0.2f;
		}
		if (maxEnemySpeed < 6f)
		{
			maxEnemySpeed += 0.2f;
		}

		
		waveNumber++;
		maxEnemies += 5;

        inCoroutine = false;
	}

    IEnumerator SpawnBossWave()
    {
        inCoroutine = true;
        UpdateUIMessage();


        yield return new WaitForSeconds(2);
        for (int i = 0; i < maxBossEnemies; i++, currentEnemies++)
        {
            if (player.Health <= 0)
            {
                break;
            }

            SpawnBasicEnemy();
            SpawnHealth();
            SpawnMissile();

            yield return new WaitForSeconds(1.5f);
        }

        if (minEnemySpeed < 4f)
        {
            minEnemySpeed += 0.2f;
        }
        if (maxEnemySpeed < 6f)
        {
            maxEnemySpeed += 0.2f;
        }


        waveNumber++;
        maxBossEnemies += 5;

        inCoroutine = false;
    }

	void UpdateUIMessage()
	{
		// Base wave every 5 waves
		if (waveNumber % 5 == 0)
		{
			message = "Boss Wave: " + waveNumber.ToString() + " Enemies: " + maxBossEnemies.ToString();
		}
		else
		{
			message = "Wave: " + waveNumber.ToString() + " Enemies: " + maxEnemies.ToString();
		}
	}

	void SpawnHealth()
	{
		// 1 in 50 chance
		if (Random.Range(0, 50) == 1)
		{
			GameObject healthPickup = Instantiate(healthPrefab) as GameObject;

			healthPickup.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
			healthPickup.GetComponent<Spaceships2DObject>().Controller = this;
			spaceships2DGameObjects.Add(healthPickup);
		}
	}

	void SpawnAsteroid(float scale = 1)
	{
		GameObject asteroid = Instantiate(asteroidPrefab) as GameObject;

        asteroid.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
        asteroid.GetComponent<Spaceships2DObject>().Controller = this;
        asteroid.GetComponent<Asteroid>().Damage = (int)(10f * scale);
        asteroid.transform.localScale *= scale;
        spaceships2DGameObjects.Add(asteroid);
	}

    void SpawnBasicEnemy()
    {
        GameObject enemy = Instantiate(basicEnemyPrefab) as GameObject;

        enemy.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
        enemy.GetComponent<Spaceships2DObject>().Controller = this;
        spaceships2DGameObjects.Add(enemy);
    }

	void SpawnMissile()
	{
		// 1 in 50 chance
		if (Random.Range(0, 50) == 2)
		{
			GameObject missilePickup = Instantiate(missilePickupPrefab) as GameObject;
		
			missilePickup.GetComponent<Spaceships2DObject>().Instantiate(RandomSpawnLocation());
			missilePickup.GetComponent<Spaceships2DObject>().Controller = this;
			spaceships2DGameObjects.Add(missilePickup);
		}
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