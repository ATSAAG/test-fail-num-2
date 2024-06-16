using Godot;

namespace TestMovements.Server;

public partial class NetworkEntity : CharacterBody2D
{
    [Export]
    public bool IsSyncEnabled = true;
    [Export]
    public float Health { get; set; }
    [Export]
    public float Speed { get; set; } 
    [Export]
    public bool IsAlive { get; set; } = true;

    [Export] public bool isAttacking { get; set; } 
    public override void _Ready()
    {
        if (IsSyncEnabled && GetTree().GetMultiplayer().IsServer() && GetTree().GetMultiplayer().HasMultiplayerPeer())
        {
            // Set the network master
            SetMultiplayerAuthority(GetTree().GetMultiplayer().GetUniqueId());
        }
    }

    public void _Process(float delta)
    {
        if (IsSyncEnabled && GetTree().GetMultiplayer().IsServer()  && GetTree().GetMultiplayer().HasMultiplayerPeer())
        {
            Rpc(nameof(SyncState), Position, Rotation, Health, IsAlive, Speed, isAttacking);
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void SyncState(Vector2 position, float rotation, float health, bool isAlive, float speed, bool attacking)
    {
        //if (GetTree().GetMultiplayer().IsServer()) return; 
        Position = position;
        Rotation = rotation;
        Health = health;
        IsAlive = isAlive;
        Speed = speed;
        isAttacking = attacking;
        if (!IsAlive)
        {
            QueueFree();
        }
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void Hit(float damage)
    {
        if (!IsAlive) return;

        Health -= damage;
        
        if (Health <= 0)
        {
            
            Health = 0;
            IsAlive = false;
            //QueueFree();
            
            Rpc(nameof(RemoveEntity));
        }
        Rpc(nameof(SyncState), Position, Rotation, Health, IsAlive, Speed, isAttacking);
        
    }
    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public void RemoveEntity()
    {
        QueueFree();
    }
}