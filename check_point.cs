using Godot;
using System;
using TestMovements;

public partial  class check_point : Node2D
{
	[Export] public bool Spawnpoint = false;

	public bool Activated = false;

	public void Activate()
	{
		CheckPointManager.CheckPoint = this;
		Activated = true;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void _on_area_2d_area_entered(Area2D area)
	{
		if (area.GetParent() is player && !Activated)
		{
			GD.Print("checkpoint taken");
			Activate();
			SaveLoadManager.SaveGame("save");
		}
	}

}






