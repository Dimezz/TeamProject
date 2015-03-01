using UnityEngine;
using System.Collections;

// Game objects which can shoot e.g player and enemies
public interface IShooter
{
    string ShooterType
    {
        get;
    }

	void RemoveMissile(GameObject missile, bool hitObject = false);
}