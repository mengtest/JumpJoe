using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour {

	public enum BoundsBehviour
	{
		Nothing,
		Constrain,
		Kill
	}
	
	public BoxCollider2D Bounds;
	public BoundsBehviour Above;
	public BoundsBehviour Below;
	public BoundsBehviour Left;
	public BoundsBehviour Right;
	
	private Player _player;
	private BoxCollider2D _boxCollider;

	// Use this for initialization
	void Start () 
	{
		_player = GetComponent<Player>();
		_boxCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_player.IsDead) 
			return;
			
		var colliderSize = new Vector2(_boxCollider.size.x * Mathf.Abs(transform.localScale.x), _boxCollider.size.y * Mathf.Abs(transform.localScale.y)) / 2;
		
		if (Above != BoundsBehviour.Nothing && transform.position.y + colliderSize.y > Bounds.bounds.max.y)
			ApplyBoundsBehavior(Above, new Vector2(transform.position.x, Bounds.bounds.max.y - colliderSize.y));
			
		if (Below != BoundsBehviour.Nothing && transform.position.y - colliderSize.y < Bounds.bounds.min.y)
			ApplyBoundsBehavior(Below, new Vector2(transform.position.x, Bounds.bounds.min.y + colliderSize.y));
			
		if (Right != BoundsBehviour.Nothing && transform.position.x + colliderSize.x > Bounds.bounds.max.x)
			ApplyBoundsBehavior(Right, new Vector2(Bounds.bounds.max.x - colliderSize.x, transform.position.y)); 
			
		if (Left != BoundsBehviour.Nothing && transform.position.x - colliderSize.x < Bounds.bounds.min.x)
			ApplyBoundsBehavior(Left, new Vector2( Bounds.bounds.min.x + colliderSize.x, transform.position.y));
	}
	
	private void ApplyBoundsBehavior(BoundsBehviour behavior, Vector2 constrainedPosition)
	{
		if (behavior == BoundsBehviour.Kill)
		{
			//LevelManager.Instance.KillPlayer();
			return;
		}
		
		transform.position = constrainedPosition;
	}
}
