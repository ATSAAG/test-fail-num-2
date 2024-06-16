using Godot;
using System;
using TestMovements;

public partial class ServeurBrowserInfo : HBoxContainer
{
	public ServeurInformation ServeurInformation;

	[Signal]
	public delegate void JoinGameEventHandler(string ip);
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
	private void _on_join_serveur_button_down()
	{GD.Print("aaaaaaaa");
		EmitSignal(SignalName.JoinGame, GetNode<Label>("IP").Text);
	}

}





