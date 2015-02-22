/*
 * Jordan Rowe: 21/02/2015
 * 
 * The UserInterface class controls the players lives, score and timer.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UserInterface : MonoBehaviour 
{
	private GameObject healthText;
	private GameObject scoreText;
	private GameObject timeText;
	private GameObject messageText;
	private GameObject missilesText;

	void Start()
	{
		healthText = GameObject.FindGameObjectWithTag("UIHealth");
		scoreText = GameObject.FindGameObjectWithTag("UIScore");
		timeText = GameObject.FindGameObjectWithTag("UITime");
		messageText = GameObject.FindGameObjectWithTag("UIMessage");
		missilesText = GameObject.FindGameObjectWithTag("UIMissiles");
	}

	public void UpdateMessage(string message)
	{
		messageText.GetComponent<Text>().text = "Incomming Transmission: " + message;
	}
	
	public void UpdateLives(int lives)
	{
		healthText.GetComponent<Text> ().text = "Health: " + lives;
	}

	public void UpdateScore(int score)
	{
		scoreText.GetComponent<Text> ().text = "Score: " + score;
	}

	public void UpdateTimer(float time)
	{
		timeText.GetComponent<Text>().text = string.Format("Time: {0}", (int)time);
	}

	public void UpdateMissileCount(int missiles)
	{
		missilesText.GetComponent<Text>().text = "Missiles: " + missiles;
	}
}
