using Godot;

using TestMovements;

public class CheckPointManager
{
    public static check_point CheckPoint;
  
    public static player _player1;
    public static player _player2;

    [Rpc(MultiplayerApi.RpcMode.AnyPeer)]
    public static void respawn()
    {
        if (CheckPoint != null)
        {
            MultiplayerManagment.multiplayerManagment.LoadLevel("res://world.tscn");

            _player1.Position = CheckPoint.Position;
            _player2.Position = CheckPoint.Position.MoveToward(CheckPoint.Position, 10);
            
            
            
        }
    }
}