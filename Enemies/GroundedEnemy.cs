using System.Linq;
using Godot;
using TestMovements.Enemies;
using TestMovements.Server;
using Vector2 = Godot.Vector2;

public abstract partial class GroundedEnemy : NetworkEntity
{
	//public float Health;
	//protected float Speed;
	protected RayCast2D[] _rayCasts;
	protected AnimatedSprite2D _sprite;
	public float Gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	//protected bool isAttacking = false;
	
	

	public override void _PhysicsProcess(double delta)
	{
		if (   GetTree().GetMultiplayer().HasMultiplayerPeer())
		//IsSyncEnabled && GetTree().GetMultiplayer().IsServer() &&
		{
			if (IsAlive)
			{
				Godot.Vector2 velocity = Move();
									// Add the gravity
									if (!IsOnFloor())
										velocity.Y += Gravity * (float)delta;
									CheckRaycasts();
									//Rpc(nameof(HandleAnimations));
									HandleAnimations();
									Velocity = velocity;
									MoveAndSlide();
									Rpc(nameof(SyncState), Position, Rotation, Health, IsAlive, Speed, isAttacking);
			}

			if (!IsAlive)
			{
				die();
			}
			
			
		}
		
	}

	public abstract Godot.Vector2 Move();
	public abstract void CheckRaycasts();

	public abstract void HandleAnimations();

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void GetHit(float damage)
	{Hit(damage);
		
		/*GD.Print(GetTree().GetMultiplayer().IsServer());
		if (GetTree().GetMultiplayer().IsServer())
		{
			GD.Print($"Damage applied on server: {damage}");
			Hit(damage);
		}
		else
		{
			GD.Print($"Client sending damage request to server: {damage}");
			RpcId(1, nameof(GetHit), damage);
		}*/
		
		/*Health -= damage;
		if (Health <= 0)
		{
			/*var id = 0;
			MultiplayerManagment.Enemies.Remove(MultiplayerManagment.Enemies.Where(i => i.Id == id).First<EnemyInfo>());
			var playerr = GetTree().GetNodesInGroup("Player");
			foreach (var i in playerr)
			{
				//GetNode<Label>("Label").Text = "*disconected*";
				GD.Print(i.Name);
				if (i.Name == id.ToString())
				{
					i.QueueFree();
				}
			}#1#

			die();
			//RpcId(Multiplayer.MultiplayerPeer.GetUniqueId(),"die");
			*/


	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	public void die()
	{
		QueueFree();
	}
}
