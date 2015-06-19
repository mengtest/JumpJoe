using UnityEngine;
using System.Collections;

public class CheckPoint : MonoBehaviour 
{
	public void Start()
	{
		
	}
	
	public void PlayerHitCheckPoint()
	{
		StartCoroutine(PlayerHitCheckPointCo(LevelManager.Instance.CurrentTimeBonus));
	}
	
	private IEnumerator PlayerHitCheckPointCo(int bonus)
	{
		FloatingText.Show("CheckPoint!", "CheckPointText", new CenteredTextPositioner(.5f));
		yield return new WaitForSeconds(.5f);
		FloatingText.Show(string.Format("+{0} time bonus!", bonus), "CheckPointText", new CenteredTextPositioner(.5f));
	}
	
	public void PlayerLeftCheckPoint()
	{
		
	}
	
	public void SpawnPlayer(Player player)
	{
		player.RespawnAt(transform);
	}
	
	public void AssignObjectToCheckPoint()
	{
		
	}
	
}
