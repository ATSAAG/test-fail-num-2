using Godot;
using System;
using TestMovements.Enemies;

public partial class RunningFrog : GroundedEnemy
{
	public override void _Ready()
	{
		Speed = 100.0f;
		_rayCasts = new RayCast2D[8];
		_rayCasts[0] = GetNode<RayCast2D>("RayCast2D");
		_rayCasts[1] = GetNode<RayCast2D>("RayCast2D2");
		_rayCasts[2] = GetNode<RayCast2D>("RayCast2D3");
		_rayCasts[3] = GetNode<RayCast2D>("RayCast2D4");
		_rayCasts[4] = GetNode<RayCast2D>("RayCast2D5");
		_rayCasts[5] = GetNode<RayCast2D>("RayCast2D6");
		_rayCasts[6] = GetNode<RayCast2D>("RayCast2D7");
		_rayCasts[7] = GetNode<RayCast2D>("RayCast2D8");
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		isAttacking = false;
		Health = 10;
	}
	
	public override Godot.Vector2 Move()
	{
		Vector2 velocity = Velocity;
		velocity.X = Speed;

		return velocity;
	}
	public override void CheckRaycasts()
	{
		int i = 0;
		if (Speed > 0)
		{
			while (i < 3 && !_rayCasts[i].IsColliding())
			{
				i++;
			}
			if (i < 3 | !_rayCasts[7].IsColliding())
			{
				Speed *= -1;
			}
		}
		else
		{
			i = 3;
			while (i < 6 && !_rayCasts[i].IsColliding())
			{
				i++;
			}
			if (i < 6 | !_rayCasts[6].IsColliding())
			{
				Speed *= -1;
			}
		}
	}

	public override void HandleAnimations()
	{
		_sprite.FlipH = Speed < 0;
		_sprite.Play("Run");
	}
	
	private void _on_hitboxe_body_entered(Node2D body)
	{
		if (body is CharacterBody2D)
		{
			((player)body).TakeHit(5);
		}
	}
}

