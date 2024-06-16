	using Godot;
using System;

public partial class SceneManager : Node2D
{

	[Export] private PackedScene playerScene;
	[Export] private PackedScene enemyScene;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		int index = 0;
		foreach (var i in MultiplayerManagment.Players)
		{
			player currentPlayer = playerScene.Instantiate<player>();
			currentPlayer.Name = i.Id.ToString();
			currentPlayer.SetPlayerName(i.Name);
			AddChild(currentPlayer);
			foreach (Node2D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawn"))
			{
				if (int.Parse(spawnPoint.Name)== index)
				{
					currentPlayer.GlobalPosition = spawnPoint.GlobalPosition;
				}
			}

			index ++;
		}
	}

	private void SpawnEnemies()
	{
		int index = 0;
		foreach (var i in MultiplayerManagment.Enemies)
		{
			GroundedEnemy currentEnemy = enemyScene.Instantiate<GroundedEnemy>();
			currentEnemy.Name = i.Id.ToString();
			
			AddChild(currentEnemy);
			// foreach (Node2D spawnPoint in GetTree().GetNodesInGroup("PlayerSpawn"))
			// {
			// 	if (int.Parse(spawnPoint.Name)== index)
			// 	{
			// 		currentEnemy.GlobalPosition = spawnPoint.GlobalPosition;
			// 	}
			// }

			index += 1;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
