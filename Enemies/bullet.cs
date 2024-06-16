using Godot;
using System;

public partial class bullet : GroundedEnemy
{
	private RayCast2D[] _rayCasts;
	private float speed = 100.0f;
	public int Direction = -1; // aller à gauche
	//public int AutreDirection = 1; // aller à droite
	private bool launched = false;
	public player player;
	
	// player
	// public Camera2D camera =  new Camera2D();
	// AddChild(camera);
	
	// multijoueur
	// var scene = ResourceLoader.Load<PackedScene>("res://world.tscn").Instantiate<Node2D>();
	// GetTree().Root.AddChild(scene);
	
	public override void _Ready()
	{
		GD.Print("Missile creee, parent name: " + GetParent().GetNode("World").Name);
		_rayCasts = new RayCast2D[1];
		_rayCasts[0] = GetNode<RayCast2D>("RayCast2D");
		//SetAsToplevel(true);
		boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
		GD.Print("new missile creee, boss testé" + boss_testee.Name);
		GlobalPosition = new Vector2(boss_testee.GlobalPosition.X, boss_testee.GlobalPosition.Y - 65);
		
	}
	
	public override void _PhysicsProcess(double delta)
	{
		//base._PhysicsProcess(delta);
		
		Vector2 velocity = Velocity;
		velocity.X += speed * Direction * (float)delta;
		Velocity = velocity;
		//GD.Print("missile move" + velocity);
		MoveAndSlide();
		//CheckRaycasts();
		
		//GD.Print("Missile process" + Position);
		
		if (Position.X<1)
		{
			QueueFree();
			boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
			boss_testee.isMissileLaunched=false;
		}
	}


	
	
	 public override Vector2 Move()
	{
		// j'ai pas de delta rien a faire ici
		//Vector2 velocity = Velocity;
		//velocity.X = Speed * Direction;
		//MoveAndSlide();
		//this.GlobalPosition += Velocity;
		return Velocity;
	}
	
	private void _on_visible_on_screen_enabler_2d_screen_exited()
	{
		/*QueueFree();
		boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
		boss_testee.isMissileLaunched=false;*/
	}
	
	public override void CheckRaycasts()
	{ 
	}
	
	private void _on_hitboxe_body_entered(Node2D body)
	{
		if (body is player)
		{
			((player)body).TakeHit(5);
		}
		if (!(body is boss_2))
		{
			QueueFree();
			GD.Print("Missile detruite avec " +body);
			boss_2 boss_testee = GetParent().GetNode("World").GetNode<boss_2>("boss_2");
			GD.Print("boss_testee" + boss_testee );
			boss_testee.isMissileLaunched=false;
		}
		
	}
	
	public override void HandleAnimations()
	{
		_sprite.Play("default");	
	}
	
	
}



