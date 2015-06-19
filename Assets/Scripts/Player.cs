using UnityEngine;

public class Player : MonoBehaviour 
{
	private bool _isFacingRight;
	private CharacterController2D _controller;
	private float _normalizedHorizontalSpeed;
	
	public float MaxSpeed = 8;
	public float SpeedAccelerationOnGround = 8f;
	public float SpeedAccelerationInAir = 5f;
	public bool IsDead { get; private set; }
	
	public void Awake()
	{
		_controller = GetComponent<CharacterController2D>();
		_isFacingRight = transform.localScale.x > 0;
	}
	
	public void Update()
	{
		if (!IsDead)
		{
			HandleInput();
		}
		
		var movementFactor = _controller.State.IsGrounded ? SpeedAccelerationOnGround : SpeedAccelerationInAir;	
		
		if (IsDead)
		{
			_controller.SetHorizontalForce(0);
		}
		else
		{
			_controller.SetHorizontalForce(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * MaxSpeed, Time.deltaTime * movementFactor));	
		}
		
		
	}
	
	public void FinishLevel()
	{
		enabled = false;
		_controller.enabled = false;
		GetComponent<Collider2D>().enabled = false;
	}

	public void Kill()
	{
		_controller.HandleCollisions = false;
		GetComponent<Collider2D>().enabled = false;
		IsDead = true;
		_controller.SetForce(new Vector2(0, 5));
	}
	
	public void RespawnAt(Transform spawnPoint)
	{
		if (!_isFacingRight) 
		{
			Flip();
		}
		
		_controller.HandleCollisions = true;
		GetComponent<Collider2D>().enabled = true;
		IsDead = false;
		
		transform.position = spawnPoint.position;
	}

	private void HandleInput()
	{
		
		if (Input.GetKey(KeyCode.Escape))
		{
			Application.Quit();
			return;
		}
		
		if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
		{
			_normalizedHorizontalSpeed = 1;
			if (!_isFacingRight)
				Flip(); 
		}
		else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
		{
			_normalizedHorizontalSpeed = -1;
			if (_isFacingRight)
				Flip(); 
		}
		else
		{
			_normalizedHorizontalSpeed = 0;	
		}
		
		if (_controller.CanJump && Input.GetKeyDown(KeyCode.Space)) 
		{
			_controller.Jump();
		}
	}
	
	private void Flip()
	{
		transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
		_isFacingRight = transform.localScale.x > 0;
	}
	
}
