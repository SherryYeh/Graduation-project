using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Role.PlayerV2;

namespace GameManagerSpace
{
    interface IGameManager
    {
        /// <summary>
        /// Game Flow
        /// </summary>
        void GameSetup();
        void GameStart();
        void Caught(GameObject target);
        void GameOver();

    }
}