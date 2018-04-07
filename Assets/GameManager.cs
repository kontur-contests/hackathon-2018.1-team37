using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : NetworkManager {

    public int maxPlayers = 2;
    private int playerCount = 0;
    private List<GameObject> players = new List<GameObject>();

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        var player = (GameObject)GameObject.Instantiate(playerPrefab, new Vector3(0,0,0), Quaternion.identity);
        player.GetComponent<PlayerController>().id = playerControllerId;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        players.Add(player);
        Debug.Log(playerControllerId);
    }

}
