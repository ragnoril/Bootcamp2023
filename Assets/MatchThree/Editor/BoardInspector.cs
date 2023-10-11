using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace MatchThree
{
    [CustomEditor(typeof(BoardManager))]
    public class BoardInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            BoardManager manager = (BoardManager)target;

            DrawDefaultInspector();

            GUILayout.Space(10);
            EditorGUILayout.HelpBox("Board Editor Tools", MessageType.Info);

            if (GUILayout.Button("Generate Empty GameBoard"))
            {
                manager.GameBoard = new int [manager.BoardWidth * manager.BoardHeight];
            }

            if (GUILayout.Button("Create New Board Data"))
            {
                CreateBoardData(manager);
            }

            if (GUILayout.Button("Open Board Editor"))
            {
                BoardEditor window = EditorWindow.GetWindow<BoardEditor>();
                window.Show();
            }


        }

        private void CreateBoardData(BoardManager manager)
        {
            BoardDataSO boardData = ScriptableObject.CreateInstance<BoardDataSO>();
            boardData.BoardHeight = manager.BoardHeight;
            boardData.BoardWidth = manager.BoardWidth;

            boardData.GameBoard = new int[boardData.BoardWidth * boardData.BoardHeight];
            for(int i = 0; i < manager.GameBoard.Length;i++)
            {
                boardData.GameBoard[i] = manager.GameBoard[i];
            }

            string path = "Assets/MatchThree/ScriptableObjects/Board_New.asset";
            AssetDatabase.CreateAsset(boardData, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
        }
    }
}
