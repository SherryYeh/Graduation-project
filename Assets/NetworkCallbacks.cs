using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class NetworkCallbacks : GlobalEventListener
{
    public GameObject cubePrefab;

    public override void SceneLoadLocalDone(string scene)
    {
        var spawnPos = new Vector2(Random.Range(-8, 8), 0);

        BoltNetwork.Instantiate(cubePrefab, spawnPos, Quaternion.identity);
    }
}
