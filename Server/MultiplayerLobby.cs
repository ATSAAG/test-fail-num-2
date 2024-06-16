using Godot;
using System;
using System.Linq;
using TestMovements;


public partial class MultiplayerLobby : Control
{
	[Export]private ENetMultiplayerPeer network = new ENetMultiplayerPeer();
	[Export] private int Port = 8910;
	[Export] private string Adress = "127.0.0.1";
	[Export]private ENetMultiplayerPeer peer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		Multiplayer.PeerConnected += PlayerConnected;
		Multiplayer.PeerDisconnected += PlayerDisconnected;
		Multiplayer.ConnectedToServer += ConnectedToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
		if (OS.GetCmdlineArgs().Contains("--server"))
		{
			peer = new ENetMultiplayerPeer();
			var error = peer.CreateServer(Port, 2);

			//add_player();
			if (error != Error.Ok)
			{
				GD.Print("cannot host" + $"{error}");
				return;
			}

			peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder); //optional compression to get less lag


			Multiplayer.MultiplayerPeer = peer;

			GD.Print("Waiting for player");
			GetNode<ServeurBrowser>("ServeurBrowser").SetUpBroadcast(GetNode<LineEdit>("LineEdit").Text + " serveur");

		}

		GetNode<ServeurBrowser>("ServeurBrowser").JoinGame += joinGame;
		ManageP2P();
		if (Input.IsActionJustPressed("shoot"))
		{
			//ManageDamage(EnemyGettingShotAt);
		}
		
	} 

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	private void ConnectedToServer()
	{
		GD.Print("connected to server");
		RpcId(1,"SendPlayerInformation", GetNode<LineEdit>("LineEdit").Text, Multiplayer.GetUniqueId());
	}


	private string ManageP2P()
	{
		var upnp = new Upnp();
		var discover = upnp.Discover();
		if (discover==(int)Upnp.UpnpResult.Success)
		{
			if (upnp.GetGateway().IsValidGateway())
			{
				var map_result_udp = upnp.AddPortMapping(Port,Port,"godot_udp", "UDP", 0 );
				var map_result_tcp = upnp.AddPortMapping(Port,Port,"godot_tcp", "TCP", 0 );

				if (map_result_udp != (int)Upnp.UpnpResult.Success)
				{
					upnp.AddPortMapping(Port, Port, "", "UDP");
				}
				if (map_result_tcp != (int)Upnp.UpnpResult.Success)
				{
					upnp.AddPortMapping(Port, Port, "", "TCP");
				}
			}
		}

		var external_ip = upnp.QueryExternalAddress();
		upnp.DeletePortMapping(9999, "UDP");
		upnp.DeletePortMapping(9999, "TCP");
		return external_ip;
	}

	

	
	private void ConnectionFailed()
	{
		GD.Print("connection failed");

	}

	private void PlayerConnected(long id)
	{
		GD.Print("Peer connected with ID: " + id);

	}

	private void PlayerDisconnected(long id)
	{
		GD.Print("Peer disconnected with ID: " + id);
		MultiplayerManagment.Players.Remove(MultiplayerManagment.Players.Where(i => i.Id == id).First<PlayerInfo>());
		var playerr = GetTree().GetNodesInGroup("Player");
		foreach (var i in playerr)
		{
			//GetNode<Label>("Label").Text = "*disconected*";
			GD.Print(i.Name);
			if (i.Name == id.ToString()) 
			{
				i.QueueFree();
			}
			
			
		}
		
	}
	
	
	private PackedScene player_scene = new PackedScene();

	private void add_player(int id = 1)
	{
		var player = player_scene.Instantiate();
		player.Name = Convert.ToString(id);
		CallDeferred("add_child", player);
	}

	private PackedScene enemie_scene = new PackedScene();
	private void add_enemie(int id =1)
	{
		var enemie = enemie_scene.Instantiate();
		enemie.Name = Convert.ToString(id);
		CallDeferred("add_child", enemie);
	}
	
	private void _on_host_button_down()
	{
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(Port, 2);

		//add_player();
		if (error != Error.Ok)
		{
			GD.Print("cannot host" + $"{error}");
			return;
		}

		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder); //optional compression to get less lag


		Multiplayer.MultiplayerPeer = peer;


		GD.Print("Waiting for player");
		GetNode<ServeurBrowser>("ServeurBrowser").SetUpBroadcast(GetNode<LineEdit>("LineEdit").Text + " serveur");
		SendPlayerInformation( GetNode<LineEdit>("LineEdit").Text,1);
	}


	private void _on_join_button_down()
	{
		joinGame(Adress);
	}

	private void joinGame(string ip)
	{
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(ip, Port);
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder); //optional compression to get less lag

		Multiplayer.MultiplayerPeer = peer;

		GD.Print("Joining game");
		//SendPlayerInformation("player 2",2);
	}
	
	
	
	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	public void StartGame()
	{
		GetNode<ServeurBrowser>("ServeurBrowser").Clean();
		foreach (var i in MultiplayerManagment.Players)
		{
			GD.Print(i.Name + " is playing");
		}
		var scene = ResourceLoader.Load<PackedScene>("res://world.tscn").Instantiate<Node2D>();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}


	private void _on_start_button_down()
	{
		Rpc("StartGame");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SendPlayerInformation(string name, int id)
	{
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id,
			
			
		};
		if (!MultiplayerManagment.Players.Contains(playerInfo))
		{
			MultiplayerManagment.Players.Add(playerInfo);
		}

		if (Multiplayer.IsServer())
		{
			foreach (var i in MultiplayerManagment.Players) 
			{
				Rpc("SendPlayerInformation", i.Name, i.Id);
			}
		}
	}
}


