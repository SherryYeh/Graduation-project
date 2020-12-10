using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Role.PlayerSpace;

public class GameManager : MonoBehaviour
{
    public bool isGameStarted;
    [SerializeField] Transform[] spawnPoints;

    public int LifeLeft
    {
        get
        {
            int left = 0;
            foreach (Player player in players)
            {
                if (player.team == 0 && player.isCaught == false) left++;
            }
            return left;
        }
    }

    Player[] players;
    int chosenNum;

    public void Catch(Player player)
    {
        Player[] newPlayers;
        player.isCaught = true;
    }

    public void GameOver()
    {
        isGameStarted = false;
        StartCoroutine(CountDown(5f));
        Invoke("Restart", 5f);
    }

    public void HumanWin()
    {
        Debug.Log("Human Win");
        GameOver();
    }

    public void GhostWin()
    {
        Debug.Log("Ghost Win");
        GameOver();
    }

    void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void ShuffleTeams()
    {
        chosenNum = Random.Range(0, players.Length);
        for (int i = 0; i < players.Length; i++)
        {
            players[i].team = 0;
        }
        players[chosenNum].team = 1;
        players[chosenNum].transform.GetComponent<SpriteRenderer>().color = Color.red;
    }

    public void GameStart()
    {
        Invoke("SpawnHuman", 0f);
        Invoke("SpawnGhost", 1f);
        isGameStarted = true;
    }

    void SpawnHuman()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (i == chosenNum) continue;
            players[i].transform.position = spawnPoints[0].position;
        }
    }

    void SpawnGhost()
    {
        players[chosenNum].transform.position = spawnPoints[1].position;
    }

    IEnumerator CountDown(float time)
    {
        while (time > 0)
        {
            Debug.Log(time);
            time -= 1;
            yield return new WaitForSeconds(1f);
        }
    }

    void Start()
    {
        players = FindObjectsOfType<Player>();
        ShuffleTeams();
        StartCoroutine(CountDown(3f));
        Invoke("GameStart", 3f);
    }

    void Update()
    {
        if (LifeLeft <= 0)
        {
            GhostWin();
        }
    }
}
