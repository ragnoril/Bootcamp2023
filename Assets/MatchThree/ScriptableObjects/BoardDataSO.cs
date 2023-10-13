using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MatchThree
{

    [CreateAssetMenu(fileName = "Board_", menuName = "Game Data/Board Data")]
    public class BoardDataSO : ScriptableObject
    {
        public int BoardWidth;
        public int BoardHeight;

        public int[] GameBoard;

        public Sprite[] TileSprites;

        public string TilePrefab;
        public string EmptyPrefab;
        public string ExplosionPrefab;
    }
}