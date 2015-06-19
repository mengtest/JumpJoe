using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour 
{
	public static LevelManager Instance { get; private set; }
	public Player Player { get; private set; }
	public CameraController Camera { get; private set; }
	public TimeSpan RunningTime { get { return DateTime.UtcNow - _started; } }
	public int CurrentTimeBonus 
	{ 
		get 
		{
			var secondDifference = (int) (BonusCutoffSeconds - RunningTime.TotalSeconds);
			return Mathf.Max(0, secondDifference) * BonusSecondMultiplier;		
		}
	}
	
	private List<CheckPoint> _checkpoints;
	private int _currentCheckPointIndex;
	private DateTime _started;
	private int _savedPoints;
	public CheckPoint DebugSpawn;
	public int BonusCutoffSeconds;
	public int BonusSecondMultiplier;
	
	public void Awake()
	{
		_savedPoints = GameManager.Instance.Points;
		Instance = this;
	}	
	
	public void Start()
	{
		_checkpoints = FindObjectsOfType<CheckPoint>().OrderBy(t => t.transform.position.x).ToList();
		_currentCheckPointIndex = _checkpoints.Count() > 0 ? 0 : -1;
		
		Player = FindObjectOfType<Player>();
		Camera = FindObjectOfType<CameraController>();
		
		_started = DateTime.UtcNow;
		
#if UNITY_EDITOR
		if (DebugSpawn != null)
		{
			DebugSpawn.SpawnPlayer(Player);
		}
		else if (_currentCheckPointIndex != -1)
		{
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(Player);
		}
#else
		if (_currentCheckPointIndex != -1)
		{
			_checkpoints[_currentCheckPointIndex].SpawnPlayer(Player);
		}
#endif

	}
	
	public void Update()
	{
		var isAtLastCheckPoint = _currentCheckPointIndex + 1 >= _checkpoints.Count;
		if (isAtLastCheckPoint) 
		{
			return;	
		}
		
		var distanceToNextCheckPoint = _checkpoints[_currentCheckPointIndex + 1].transform.position.x - Player.transform.position.x;
		if (distanceToNextCheckPoint >= 0) 
		{
			return;
		}
		
		_checkpoints[_currentCheckPointIndex].PlayerLeftCheckPoint();
		_currentCheckPointIndex++;
		_checkpoints[_currentCheckPointIndex].PlayerHitCheckPoint();
		
		GameManager.Instance.AddPoints(CurrentTimeBonus);
		_savedPoints = GameManager.Instance.Points;
		_started = DateTime.UtcNow;
	}
	
	public void GotoNextLevel(string levelName)
	{
		StartCoroutine(GotoNextLevelCo(levelName));
	}
	
	private IEnumerator GotoNextLevelCo(string levelName)
	{
		Player.FinishLevel();
		GameManager.Instance.Level++;
		GameManager.Instance.AddPoints(CurrentTimeBonus);
		
		if (string.IsNullOrEmpty(levelName)) 
		{
			
			FloatingText.Show("Game Complete!", "CheckPointText", new CenteredTextPositioner(.2f));
		
			yield return new WaitForSeconds(1);
			
			Application.LoadLevel("StartScreen");
		}
		else
		{
			
			FloatingText.Show("Level Complete!", "CheckPointText", new CenteredTextPositioner(.2f));
		
			yield return new WaitForSeconds(1);
			
			FloatingText.Show(string.Format("{0} points!", GameManager.Instance.Points), "CheckPointText", new CenteredTextPositioner(.1f));
			yield return new WaitForSeconds(5f);
			
			Application.LoadLevel(levelName);
		}
	}
	
	public void KillPlayer()
	{
		FloatingText.Show("DEATH", "PointsText", new FromWorldPointTextPositioner(Camera.GetComponent<Camera>(), Player.transform.position, 1.5f, 50));
		StartCoroutine(KillPlayerCo());
	}
	
	private IEnumerator KillPlayerCo()
	{
		Player.Kill();
		Camera.IsFollowing = false;
		yield return new WaitForSeconds(2);
		
		Camera.IsFollowing = true;
		
		GameManager.Instance.Lives--;
		
		if (GameManager.Instance.Lives == 0)
		{
			//Take player back to start screen
			FloatingText.Show("GAME OVER", "CheckPointText", new CenteredTextPositioner(.1f));
			yield return new WaitForSeconds(5f);
			Application.LoadLevel("StartScreen");
		}
		else
		{
			//Take player back to last checkpoint
			if (_currentCheckPointIndex != -1) 
			{	
				_checkpoints[_currentCheckPointIndex].SpawnPlayer(Player);
			}
			
			_started = DateTime.UtcNow;
			GameManager.Instance.ResetPoints(_savedPoints);
		}
		
	}
}
