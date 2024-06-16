using Godot;
using System;
using System.Collections.Generic;
using TestMovements;
using TestMovements.Enemies;

public partial class MultiplayerManagment : Node
{
	public static List<PlayerInfo> Players = new List<PlayerInfo>();

	public static List<EnemyInfo> Enemies = new List<EnemyInfo>();

	public string Path = "res://world.tscn";
	

	public static MultiplayerManagment multiplayerManagment;	
	[Export] public string LoadScene = "res://loading_screen.tscn";
	public void Die()
	{
		QueueFree();
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (multiplayerManagment == null)
		{
			multiplayerManagment = this;
		}
		else
		{
			QueueFree();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void LoadLevel(string path)
	{
		Path = path;
		PackedScene scene = ResourceLoader.Load<PackedScene>(LoadScene);
		loading_screen loadingScreen = scene.Instantiate<loading_screen>();
		GetTree().Root.AddChild(loadingScreen);
		loadingScreen.LoadLevel(path);
	}

	private Godot.Resource Info = new Resource();
	public void SaveGame(string path)
	{
		
		Path = path;
		ResourceSaver.Save(Info, path);
	}
}
