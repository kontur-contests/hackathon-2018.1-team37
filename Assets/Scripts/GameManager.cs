using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class GameManager : NetworkManager {

    public int maxPlayers = 2;
    public GameObject server;
    public GameObject playerBluePrefab;
    public GameObject playerRedPrefab;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        server = GameObject.Find("GameServer");
            
        if(server.GetComponent<ServerBehaviour>().players.Count > 0)
        {
            var player = (GameObject)GameObject.Instantiate(playerRedPrefab, new Vector3(-3.05f, -0.68f, 0), Quaternion.identity);
            player.GetComponent<PlayerController>().ID = playerControllerId;
            server.GetComponent<ServerBehaviour>().players.Add(player);
            Debug.Log(playerControllerId);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
            server.GetComponent<ServerBehaviour>().state = ServerBehaviour.State.Start;
        } else {
            var player = (GameObject)GameObject.Instantiate(playerBluePrefab, new Vector3(3.66f, -0.68f, 0), Quaternion.Euler(0, -180, 0));
            player.GetComponent<PlayerController>().ID = playerControllerId;
            server.GetComponent<ServerBehaviour>().players.Add(player);
            Debug.Log(playerControllerId);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }
    }

}
