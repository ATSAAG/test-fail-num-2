using Godot;
using System;
using System.Linq;
using System.Text.Json;
using TestMovements;

public partial class ServeurBrowser : Control
{


	[Export] 
	private PacketPeerUdp _broadcaster;
	[Export]
	private PacketPeerUdp _listener = new PacketPeerUdp();

	[Export] private int _listenPort = 8911;
	[Export] private int _hostPort = 8912;

	[Export] private string _broadcastAddress = "192.168.1.255";
	[Export]
	private PackedScene ServeurInformation;
	
	[Signal]
	public delegate void JoinGameEventHandler(string ip);

	private Timer _broadcastTimer;

	private ServeurInformation _serveurInformation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_broadcastTimer = GetNode<Timer>("BroadcastTimer");
		SetUpListener();
	}

	public void SetUpBroadcast(string name)
	{
		_broadcaster = new PacketPeerUdp();
		_serveurInformation = new ServeurInformation()
		{
			Name = name,
			NumberOfPlayer = MultiplayerManagment.Players.Count
		};
		_broadcaster.SetBroadcastEnabled(true);
		_broadcaster.SetDestAddress(_broadcastAddress, _listenPort);
		var checkPort = _broadcaster.Bind(_hostPort);
		if (checkPort == Error.Ok)
		{
			GD.Print("Bind to Broadcast Port :" + _hostPort.ToString());
		}
		else
		{
			GD.Print("Error : Not Bind to Broadcast Port");
		}
		_broadcastTimer.Start();
		//Timer start now and will send a signal every second
	}

	private void SetUpListener()
	{
		var checkPort = _listener.Bind(_listenPort, "0.0.0.0");
		GD.Print(checkPort);
		if (checkPort == Error.Ok)
		{
			GD.Print("Bind to listen Port :" + _listenPort.ToString());
			GetNode<Label>("Label Bound to Listen").Text = "Bound to Listen : true";
		}
		else
		{
			GD.Print("Error : Not Bind to listen Port");
			GetNode<Label>("Label Bound to Listen").Text = "Bound to Listen : false";
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_listener.GetAvailablePacketCount()> 0)
		{
			string serveurIp = _listener.GetPacketIP();
			int serveurPort = _listener.GetPacketPort();
			var serveurPacket = _listener.GetPacket();
			ServeurInformation information = JsonSerializer.Deserialize<ServeurInformation>(serveurPacket.GetStringFromUtf8());
			//convert from bytes array to serveurInformation
			
			GD.Print("serveur IP : " + serveurIp + "serveur port : " + serveurPort +  "serveur information" + serveurPacket.GetStringFromUtf8());

			Node CurNode = GetNode<VBoxContainer>("Panel/VBoxContainer").GetChildren().Where(x => x.Name == information.Name).FirstOrDefault();

			if (CurNode != null) 
			{
				CurNode.GetNode<Label>("PlayerConnected").Text = information.NumberOfPlayer.ToString();
				CurNode.GetNode<Label>("IP").Text = serveurIp;
				
				
				return;
			}
			
			
			ServeurBrowserInfo serveurInformation = ServeurInformation.Instantiate<ServeurBrowserInfo>();
			serveurInformation.Name = information.Name;
			serveurInformation.GetNode<Label>("Name").Text = serveurInformation.Name;
			serveurInformation.GetNode<Label>("IP").Text = serveurIp;
			serveurInformation.GetNode<Label>("PlayerConnected").Text = information.NumberOfPlayer.ToString();
			GetNode<VBoxContainer>("Panel/VBoxContainer").AddChild(serveurInformation);

			serveurInformation.JoinGame += _on_join_game;
#pragma warning disable CS8974 // Converting method group to non-delegate type
			serveurInformation.GetNode<Button>("VJoinServeur").Text += _on_join_game;
#pragma warning restore CS8974 // Converting method group to non-delegate type
		}
	}

	private void _on_join_game(string ip)
	{
		EmitSignal(SignalName.JoinGame, ip);
	}
	
	private void _on_broadcast_timer_timeout()
	{
		GD.Print("Broadcasting Game");
		_serveurInformation.NumberOfPlayer = MultiplayerManagment.Players.Count;
		//update the number of player connected

		string json = JsonSerializer.Serialize(_serveurInformation);
		var packet = json.ToUtf8Buffer();
		_broadcaster.PutPacket(packet);
	}

	public void Clean()
	{
		_listener.Close();
		_broadcastTimer.Stop();
		if (_broadcaster != null)
		{
			_broadcaster.Close();
		}
	}
}
