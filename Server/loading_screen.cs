using Godot;
using System;

public partial class loading_screen : Control
{

	private string _path;

	private bool _loading;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_loading)
		{
			var progress = new Godot.Collections.Array();
			var status = ResourceLoader.LoadThreadedGetStatus(_path, progress);
			if (status == ResourceLoader.ThreadLoadStatus.InProgress)
			{
				GetNode<ProgressBar>("ProgressBar").Value = (double)progress[0] * 100;
			}
			else
			{
				if (status == ResourceLoader.ThreadLoadStatus.Loaded)
				{
					SetProcess(false);
					GetNode<ProgressBar>("ProgressBar").Value = 100;
					ChangeScene(ResourceLoader.LoadThreadedGet(_path) as PackedScene);
				}
				else
				{
					if (status == ResourceLoader.ThreadLoadStatus.InvalidResource)
					{
						ResourceLoader.LoadThreadedRequest(_path);
					}
				}
			}
			
		}
		
	}

	public void ChangeScene(PackedScene res)
	{


		var rootNode = GetTree().Root;
		foreach (var node in GetTree().Root.GetChildren())
		{
			if (node is Node2D || node is Node3D || node is Control)
			{
				GetTree().Root.RemoveChild(node);
							node.QueueFree();
			}
			
		}
		Node cur = res.Instantiate();
		rootNode.AddChild(cur);
		QueueFree();
	}
	public void LoadLevel(string path)
	{
		_path = path;
		Show();
		if (ResourceLoader.HasCached(path))
		{
			ResourceLoader.LoadThreadedGet(path);
			_loading = true;
		}
		else
		{
			ResourceLoader.LoadThreadedRequest(path);
			_loading = true;
		}

		_loading = true;
	}
}
