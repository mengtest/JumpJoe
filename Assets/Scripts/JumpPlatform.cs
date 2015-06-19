using UnityEngine;
using System;
using System.Collections;

public class JumpPlatform : MonoBehaviour 
{
	public float JumpMagnitude = 20;
	
	private SpriteRenderer _renderer;
	
	public void Start()
	{
		_renderer = GetComponent<SpriteRenderer>();
	}
	
	public void ControllerEnter2D(CharacterController2D controller)
	{
		controller.SetVerticalForce(JumpMagnitude);	
		
		_renderer.color = Color.blue;
	}
	
	public void ControllerExit2D(CharacterController2D controller)
	{
		StartCoroutine(ChangeColor());
	}
	
	IEnumerator ChangeColor()
	{
		yield return new WaitForSeconds(.1f);
		_renderer.color = Color.green;
	}	
	
}
