using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThree
{

    public class GameManager : MonoBehaviour
    {
        public BoardDataSO[] Boards;

        public int LevelIndex;


        public BoardManager Board;



        private void Start()
        {
            Board.Init(this, Boards[LevelIndex % Boards.Length]);
        }

    }
}