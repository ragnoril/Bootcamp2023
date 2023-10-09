using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MatchThree
{
    public class BoardManager : MonoBehaviour
    {
        private GameManager _gameManager;

        private int _boardWidth;
        private int _boardHeight;

        private int[] _gameBoard;
        /*
        public GameObject TilePrefab;
        public GameObject EmptyTilePrefab;
        public Sprite[] TileSprites;
        */
        private BoardDataSO _boardData;

        private Camera _camera;

        private Tile _selectedTile;
        private Tile _swapTile;

        private int _moveCount;

        public event Action<int> OnBoardMove;

        public void Init(GameManager gm, BoardDataSO boardData)
        {
            _gameManager = gm;
            _camera = Camera.main; 
            _boardData = boardData;

            InitBoard();
            RenderBoard();
        }

        private void FillBoardRandomly()
        {
            for (int j = 0; j < _boardHeight; j++)
            {
                for (int i = 0; i < _boardWidth; i++)
                {
                    _gameBoard[GetBoardPosition(i, j)] = UnityEngine.Random.Range(0, _boardData.TileSprites.Length);
                }
            }
        }

        private void RenderBoard()
        {
            for(int j = 0; j < _boardHeight; j++)
            {
                for(int i  = 0; i < _boardWidth; i++)
                {
                    GameObject emptyTile = Instantiate(_boardData.EmptyPrefab);
                    emptyTile.transform.SetParent(transform);
                    emptyTile.transform.localPosition = new Vector3(i, -j, 0f);

                    int tileId = _gameBoard[GetBoardPosition(i, j)];
                    CreateTile(tileId, i, j);
                }
            }
        }

        private void CreateTile(int tileId, int x, int y)
        {
            GameObject tileObject = Instantiate(_boardData.TilePrefab);
            tileObject.transform.SetParent(transform);
            tileObject.transform.localPosition = new Vector3(x, -y, 0f);

            Tile tile = tileObject.GetComponent<Tile>();
            tile.TileType = tileId;
            tile.SetSprite(_boardData.TileSprites[tileId]);
        }

        private int GetBoardPosition(int x, int y)
        {
            return (_boardWidth * y) + x;
        }

        public void InitBoard()
        {
            _moveCount = 0;
            _boardWidth = _boardData.BoardWidth;
            _boardHeight = _boardData.BoardHeight;

            _gameBoard = new int[_boardWidth * _boardHeight];

            FillBoardRandomly();

        }

        private void Update()
        {
#if UNITY_IOS || UNITY_ANDROID
            HandleMobileInput();
#else
            HandlePCInput();
#endif
        }

        private void SwapSelectedTiles(Vector2 swipePos)
        {
            int swipeX = (int)Mathf.Clamp(Mathf.Round(swipePos.x - _selectedTile.transform.position.x), -1f, 1f);
            int swipeY = (int)Mathf.Clamp(Mathf.Round(_selectedTile.transform.position.y - swipePos.y), -1f, 1f);

            int selectedX = _selectedTile.GetX();
            int selectedY = _selectedTile.GetY();

            int swapX = selectedX + swipeX;
            int swapY = selectedY + swipeY;

            _swapTile = GetTile(swapX, swapY);

            if (_swapTile != null)
            {
                int selectedPosition = GetBoardPosition(selectedX, selectedY);
                int swapPosition = GetBoardPosition(swapX, swapY);

                _gameBoard[selectedPosition] = _swapTile.TileType;
                _gameBoard[swapPosition] = _selectedTile.TileType;

                Vector3 tempPos = _swapTile.transform.position;
                _swapTile.transform.position = _selectedTile.transform.position;
                _selectedTile.transform.position = tempPos;

                _moveCount += 1;
                OnBoardMove?.Invoke(_moveCount);

                CheckForCombos();
            }

            _selectedTile = null;
            _swapTile = null;
        }

        private void HandleMobileInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    RaycastHit2D hitInfo = Physics2D.Raycast(_camera.ScreenToWorldPoint(touch.rawPosition), Vector2.zero);
                    if (hitInfo.collider != null && hitInfo.collider.tag == "Tile")
                    {
                        _selectedTile = hitInfo.collider.GetComponent<Tile>();
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    Vector2 touchPos = _camera.ScreenToWorldPoint(touch.position);
                    SwapSelectedTiles(touchPos);
                }

            }
        }

        private void HandlePCInput()
        { 
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hitInfo.collider != null && hitInfo.collider.tag == "Tile")
                {
                    _selectedTile = hitInfo.collider.GetComponent<Tile>();
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                Vector2 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                SwapSelectedTiles(mousePos);
            }
        }

        private void CheckForCombos()
        {
            
        }

        private Tile GetTile(int x, int y)
        {
            Collider2D tileHit =  Physics2D.OverlapPoint(new Vector2(transform.position.x + x, transform.position.y - y));
            if (tileHit != null && tileHit.tag == "Tile") return tileHit.GetComponent<Tile>();

            return null;

        }
    }
}