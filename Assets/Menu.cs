using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Bolt;
using Bolt.Matchmaking;
using UdpKit;

public class Menu : GlobalEventListener
{
    //called from Host Game Button
    public void StartServer()
    {
        BoltLauncher.StartServer();
    }

    public override void BoltStartDone()
    {
        BoltMatchmaking.CreateSession(sessionID: "test", sceneToLoad: "Game");
    }

    //called from Join Game Button
    public void StartClient()
    {
        BoltLauncher.StartClient();
    }

    public override void BoltStartDone()
    {
        if(BoltNetwork.IsServer)
        {
            string matchName = "Test Match";

            BoltNetwork.SetServerInfo(matchName,null);
            BoltNetwork.LoadScene("SampleScene");
        }
    }

    //called when a room is created or destroyed, As Well As ever few seconds
    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;
            /*
            if(photonSession.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(photonSeesion);
            }
            */
        }

    }
}
