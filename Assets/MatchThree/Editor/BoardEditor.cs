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
        private List<string> _brushes;
        private Sprite _spriteToAdd;

        private int _selectedBrush;

        private bool _isBoardSet;

        private Vector2 _scrollPos = Vector2.zero;


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
            _brushes = new List<string>();
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
                _brushes.Add(_spriteToAdd.name);
                _spriteToAdd = null;
            }
            EditorGUILayout.EndHorizontal();

            _selectedBrush = EditorGUILayout.Popup("Selected Tile", _selectedBrush, _brushes.ToArray());

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
                _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
                DrawGameBoard();
                EditorGUILayout.EndScrollView();

                GUILayout.Space(10);
                if (GUILayout.Button("Save Game Board"))
                {
                    SaveGameBoardData();
                }
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
                    if (GUILayout.Button(CreateButtonTexture(_gameBoard[(_boardWidth * j) + i])))
                    {
                        _gameBoard[(_boardWidth * j) + i] = _selectedBrush;
                    }
                }

                GUILayout.EndHorizontal();

            }
            GUILayout.EndVertical();
        }

        private GUIContent CreateButtonTexture(int tileId)
        {
            GUIContent buttonContent;

            int textureWidth = 50;
            int textureHeight = 50;
            if (_sprites.Count > 0)
            {
                textureWidth = (int)_sprites[0].rect.width;
                textureHeight = (int)_sprites[0].rect.width;
            }

            switch (tileId)
            {
                case -1:
                    Texture2D emptyTexture = new Texture2D(textureWidth, textureHeight);
                    buttonContent = new GUIContent(emptyTexture);
                    break;
                default:
                    Texture2D buttonTexture = new Texture2D((int)_sprites[tileId].rect.width, (int)_sprites[tileId].rect.height);
                    var pixels = _sprites[tileId].texture.GetPixels((int)_sprites[tileId].textureRect.x, (int)_sprites[tileId].textureRect.y, (int)_sprites[tileId].rect.width, (int)_sprites[tileId].rect.height);
                    buttonTexture.SetPixels(pixels);
                    buttonTexture.Apply();
                    buttonContent = new GUIContent(buttonTexture);
                    break;
            }

            return buttonContent;
        }

        private void GenerateBoard()
        {
            _gameBoard = new int[_boardHeight * _boardWidth];
            for (int i = 0; i < _gameBoard.Length; i++)
                _gameBoard[i] = -1;

            _isBoardSet = true;
        }

        private void SaveGameBoardData()
        {
            Debug.Log("Create Board Data SO");
            BoardDataSO boardData = ScriptableObject.CreateInstance<BoardDataSO>();
            boardData.BoardWidth = _boardWidth;
            boardData.BoardHeight = _boardHeight;

            boardData.GameBoard = new int[_boardWidth * _boardHeight];
            for(int i = 0; i < _gameBoard.Length; i++)
            {
                boardData.GameBoard[i] = _gameBoard[i];
            }

            boardData.TileSprites = new Sprite[_sprites.Count];
            for(int i = 0; i < _sprites.Count; i++)
            {
                boardData.TileSprites[i] = _sprites[i];
            }

            string path = "Assets/MatchThree/ScriptableObjects/Board_New.asset";
            AssetDatabase.CreateAsset(boardData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();

        }
    }
}