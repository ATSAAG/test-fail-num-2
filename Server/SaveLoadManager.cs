using Godot;
using System;
using System.Text.Json.Serialization;
using Godot.Collections;
using Newtonsoft.Json;

public partial class SaveLoadManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public static void SaveGame(string name)
	{
		DirAccess directory = DirAccess.Open("user://");
		if (!directory.DirExists("Saves"))
		{
			directory.MakeDir("Saves");
		}

		directory = DirAccess.Open("user://Saves");
		if (!directory.DirExists(name))
		{
			directory.MakeDir(name);
		}

		Dictionary<string, string> saveGameData = new Dictionary<string, string>();
		saveGameData.Add("Path", MultiplayerManagment.multiplayerManagment.Path);
		foreach (var c in saveGameData.Values)
		{
			GD.Print(c);
		}
		
		string saveJson = JsonConvert.SerializeObject(saveGameData);

		FileAccess file = FileAccess.Open($"user://Saves/{name}.json", FileAccess.ModeFlags.Write);
		file.StoreString(saveJson);
		file.Close();
	}

	public static Dictionary<string, string> LoadGame(string name)
	{
		FileAccess file = FileAccess.Open($"user://Saves/{name}/{name}.json", FileAccess.ModeFlags.Read);
		string content = file.GetAsText();
		return JsonConvert.DeserializeObject(content) as Dictionary<string,string>;
	}
}
