using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MatchThree
{
    public class BoardEditor : EditorWindow
    {
        private int _boardWidth;
        private int _boardHeight;
        private int[] _gameBoard;

        private List<Sprite> _sprites;
        private Sprite _spriteToAdd;

        private bool _isBoardSet;


        [MenuItem("Tools/Board Editor")]
        static void Init()
        {
            BoardEditor window = EditorWindow.GetWindow<BoardEditor>();

            window.Show();
        }

        private void OnEnable()
        {
            _isBoardSet = false;
            _sprites = new List<Sprite>();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Board Editor for Match-3 Game");
            GUILayout.Space(10);

            _boardWidth = EditorGUILayout.IntField("Board Width", _boardWidth);
            _boardHeight = EditorGUILayout.IntField("Board Height", _boardHeight);
            GUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            _spriteToAdd = EditorGUILayout.ObjectField("Tile Sprite", _spriteToAdd, typeof(Sprite)) as Sprite;
            if (GUILayout.Button("Add Sprite"))
            {
                _sprites.Add(_spriteToAdd);
                _spriteToAdd = null;
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Generate Board"))
            {
                GenerateBoard();
            }
            if (GUILayout.Button("Clear Board"))
            {
                _isBoardSet = false;
            }
            GUILayout.Space(10);

            if (_isBoardSet)
            {
                DrawGameBoard();
            }

        }

        private void DrawGameBoard()
        {
            GUILayout.BeginVertical();
            for(int j = 0; j < _boardHeight; j++)
            {
                GUILayout.BeginHorizontal();

                for(int i = 0; i < _boardWidth; i++)
                {
                    GUILayout.Button(_gameBoard[(_boardWidth * j) + i].ToString());
                }

                GUILayout.EndHorizontal();

            }
            GUILayout.EndVertical();
        }

        private void GenerateBoard()
        {
            _gameBoard = new int[_boardHeight * _boardWidth];
            for (int i = 0; i < _gameBoard.Length; i++)
                _gameBoard[i] = -1;

            _isBoardSet = true;
        }
    }
}