using Godot;
using System;

public partial class JoinPlayerManager : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_go_back_button_down()
	{
		var cur = GetTree().CurrentScene;
		var scene = ResourceLoader.Load<PackedScene>("res://Server/MainMenu.tscn").Instantiate<Control>();
		GetTree().Root.AddChild(scene);
		
		cur.QueueFree();
		GetTree().CurrentScene = scene;
	}
}



