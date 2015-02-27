/*
 * Jordan Rowe: 27/02/2015
 * 
 * The BasicEnemyController class controls the behaviour of enemies
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicEnemyController : Spaceships2DObject, IShooter
{
	public float speed = 2f;
	public GameObject missilePrefab;

	private PlayerController player;

	// Points variables
	private int pointsForDestroying;

	// Timers
	private float weaponFireRate;
	private float weaponFireTimer;
	private List<GameObject> missileList;
	
	public int Damage
	{
		get; set;
	}
	
	void Start()
	{
		pointsForDestroying = 10;
		pointsForCollision = 0;

		mainCamera = GameObject.FindGameObjectWithTag("MainCamera").camera;
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		type = Spaceships2DObjectType.Asteroid;
		
		weaponFireRate = 2f;
		weaponFireTimer = 0f;
		missileList = new List<GameObject>();
	}
	
	void FixedUpdate()
	{
		float difference = (transform.position - player.transform.position).magnitude;
		weaponFireTimer += Time.deltaTime;

		if (difference > 5f)
		{
			transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
		}
		FireMissiles();
	}

	void FireMissiles()
	{
		if (weaponFireTimer > weaponFireRate)
		{
			weaponFireTimer = 0f;

            for (Direction d = Direction.Up; d <= Direction.DownLeft; d++)
            {
                GameObject missile = Instantiate(missilePrefab) as GameObject;
                missile.GetComponent<Missile>().Fire(transform, d, this);
                missileList.Add(missile);
            }
		}
	}

	// If MonoDevelop gives you an error here change the target frame to 4.5 - Project -> Assembly-CSharp -> General
	public void RemoveMissile(GameObject missile, bool hitObject = false)
	{
		missileList.Remove(missile);
		Destroy(missile);
	}

	// More points if hit by a players missile
	public void HitByWeapon()
	{
		Controller.RemoveSpaceship2DObject(gameObject, type, pointsForDestroying);
	}
}
