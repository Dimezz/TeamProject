using UnityEngine;
using System.Collections;

// Game objects which can shoot e.g player and enemies
public interface IShooter
{
	void RemoveMissile(GameObject missile, bool hitObject = false);
}