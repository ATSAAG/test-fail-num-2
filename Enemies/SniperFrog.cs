using Godot;
using System;

public partial class SniperFrog : GroundedEnemy
{

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();

	public override void _Ready()
	{
		Speed = 300f;
		_rayCasts = new RayCast2D[1];
		_rayCasts[0] = GetNode<RayCast2D>("RayCast2D");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		Health = 10;
		isAttacking = false;
	}

	public override Vector2 Move()
	{
		return Velocity;
	}

	public override void CheckRaycasts()
	{
		if (_rayCasts[0].IsColliding())
		{
			player body = (player) _rayCasts[0].GetCollider();
			body.TakeHit(5);
			isAttacking = true;
		}
	}

	public override void HandleAnimations()
	{
		if (isAttacking)
		{
			_sprite.Play("Shoot");
			isAttacking = false;
		}
		else
		{
			_sprite.Play("Aim");
		}
	}
}
