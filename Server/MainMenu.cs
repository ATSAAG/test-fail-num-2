using Godot;
using System;

public partial class MainMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_host_button_down()
	{
		
		//GetNode<ServeurBrowser>("ServeurBrowser").Clean();
		var cur = GetTree().CurrentScene;
		var scene = ResourceLoader.Load<PackedScene>("res://Server/SaveManager.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		cur.QueueFree();
		GetTree().CurrentScene = scene;
	}


	private void _on_join_button_down()
	{var cur = GetTree().CurrentScene;
		var scene = ResourceLoader.Load<PackedScene>("res://Server/JoinPlayerManager.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		
		cur.QueueFree();
		GetTree().CurrentScene = scene;
	}


	private void _on_settings_button_down()
	{var cur = GetTree().CurrentScene;
		var scene = ResourceLoader.Load<PackedScene>("res://Server/SettingsPanel.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		
		cur.QueueFree();
		GetTree().CurrentScene = scene;
	}

}


