using Mono.Cecil;
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

        [SerializeField]
        public int BoardWidth;
        [SerializeField]
        public int BoardHeight;

        [SerializeField]
        public int[] GameBoard;
        
        /*
        public GameObject TilePrefab;
        public GameObject EmptyTilePrefab;
        public Sprite[] TileSprites;
        */
        [SerializeField]
        private BoardDataSO _boardData;

        private Camera _camera;

        private Tile _selectedTile;
        private Tile _swapTile;

        private int _moveCount;
        private int _popCount;

        public event Action<int> OnBoardMove;
        public event Action<int> OnPop;

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
            for (int j = 0; j < BoardHeight; j++)
            {
                for (int i = 0; i < BoardWidth; i++)
                {
                    GameBoard[GetBoardPosition(i, j)] = UnityEngine.Random.Range(0, _boardData.TileSprites.Length);
                }
            }
        }

        private void RenderBoard()
        {
            for(int j = 0; j < BoardHeight; j++)
            {
                for(int i  = 0; i < BoardWidth; i++)
                {
                    GameObject emptyTile = Instantiate(Resources.Load<GameObject>(_boardData.EmptyPrefab));
                    emptyTile.transform.SetParent(transform);
                    emptyTile.transform.localPosition = new Vector3(i, -j, 0f);

                    int tileId = GameBoard[GetBoardPosition(i, j)];
                    CreateTile(tileId, i, j);
                }
            }
        }

        private void CreateTile(int tileId, int x, int y)
        {
            GameObject tileObject = Instantiate(Resources.Load<GameObject>(_boardData.TilePrefab));
            tileObject.transform.SetParent(transform);
            tileObject.transform.localPosition = new Vector3(x, -y, 0f);

            Tile tile = tileObject.GetComponent<Tile>();
            tile.TileType = tileId;
            tile.SetSprite(_boardData.TileSprites[tileId]);
        }

        private int GetBoardPosition(int x, int y)
        {
            return (BoardWidth * y) + x;
        }



        public void InitBoard()
        {
            _moveCount = 0;
            _popCount = 0;
            if (_boardData != null)
            {
                BoardWidth = _boardData.BoardWidth;
                BoardHeight = _boardData.BoardHeight;
            }

            GameBoard = new int[BoardWidth * BoardHeight];

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

                GameBoard[selectedPosition] = _swapTile.TileType;
                GameBoard[swapPosition] = _selectedTile.TileType;

                Vector3 tempPos = _swapTile.transform.position;
                _swapTile.transform.position = _selectedTile.transform.position;
                _selectedTile.transform.position = tempPos;

                _moveCount += 1;
                OnBoardMove?.Invoke(_moveCount);

                CheckForCombos();
                StartCoroutine(HandleEmptySpaces());
            }

            _selectedTile = null;
            _swapTile = null;
        }

        IEnumerator HandleEmptySpaces()
        {
            yield return new WaitForSeconds(0.5f);
            CheckForEmptySpaces();
            yield return new WaitForSeconds(0.5f);
            FillEmptySpaces();
        }

        private void FillEmptySpaces()
        {
            for(int i= 0; i < BoardWidth; i++)
            {
                for(int j = 0; j < BoardHeight; j++)
                {
                    int pos = GetBoardPosition(i, j);
                    if (GameBoard[pos] < 0)
                    {
                        GameBoard[pos] = UnityEngine.Random.Range(0, _boardData.TileSprites.Length);
                        CreateTile(GameBoard[pos], i, j);
                    }
                }
            }
        }

        private void CheckForEmptySpaces()
        {
            for(int i = 0; i < BoardWidth; i++)
            {
                for(int j = (BoardHeight -2); j > -1; j--)
                {
                    if (GameBoard[GetBoardPosition(i, j)] < 0) continue;

                    Tile tile = GetTile(i, j);
                    int y = j + 1;

                    while (y < BoardHeight && GameBoard[GetBoardPosition(i,y)] < 0) 
                    {
                        tile.transform.localPosition = new Vector3(i, -y, 0f);
                        GameBoard[GetBoardPosition(i, y - 1)] = -1;
                        GameBoard[GetBoardPosition(i, y)] = tile.TileType;

                        y += 1;
                    }
                }
            }
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
            _popCount += CheckCombosForTile(_selectedTile);
            _popCount += CheckCombosForTile(_swapTile);
        }

        private int CheckCombosForTile(Tile comboTile)
        {
            int popCount = 0;
            int comboX = comboTile.GetX();
            int comboY = comboTile.GetY();

            List<Tile> comboH = CheckCombosHorizontal(comboX, comboY, comboTile.TileType);
            List<Tile> comboV = CheckCombosVertical(comboX, comboY, comboTile.TileType);

            bool isComboTilePops = false;
            if (comboH.Count > 1)
            {
                foreach (Tile tile in comboH)
                {
                    popCount += 1;
                    PopTile(tile);
                }

                isComboTilePops = true;
            }

            if (comboV.Count > 1)
            {
                foreach(Tile tile in comboV)
                {
                    popCount += 1;
                    PopTile(tile);
                }

                isComboTilePops = true;
            }

            if (isComboTilePops)
            {
                popCount += 1;
                PopTile(comboTile);
            }

            return popCount;
        }

        private void PopTile(Tile tile)
        {
            OnPop?.Invoke(_popCount);
            GameBoard[GetBoardPosition(tile.GetX(), tile.GetY())] = -1;

            GameObject explosion = Instantiate(Resources.Load<GameObject>(_boardData.ExplosionPrefab));
            explosion.transform.position = tile.transform.position;

            Destroy(explosion.gameObject, 0.5f);
            Destroy(tile.gameObject);
        }

        private List<Tile> CheckCombosHorizontal(int x, int y, int tileType)
        {
            List<Tile> comboList = new List<Tile>();

            int preX = x - 1;
            int postX = x + 1;

            while(preX > -1 || postX < BoardWidth)
            {
                if (preX > -1)
                {
                    int prePos = GetBoardPosition(preX, y);
                    if (GameBoard[prePos] == tileType)
                    {
                        Tile tile = GetTile(preX, y);
                        if (tile != null)
                        {
                            comboList.Add(tile);
                            preX -= 1;
                        }
                    }
                    else
                    {
                        preX = -1;
                    }
                }

                if (postX < BoardWidth)
                {
                    int postPos = GetBoardPosition(postX, y);
                    if (GameBoard[postPos] == tileType)
                    {
                        Tile tile = GetTile(postX, y);
                        if (tile != null)
                        {
                            comboList.Add(tile);
                            postX += 1;
                        }
                    }
                    else
                    {
                        postX = BoardWidth;
                    }
                }
            }

            return comboList;
        }

        private List<Tile> CheckCombosVertical(int x, int y, int tileType)
        {
            List<Tile> comboList = new List<Tile>();

            int preY = y - 1;
            int postY = y + 1;

            while (preY > -1 || postY < BoardHeight)
            {
                if (preY > -1)
                {
                    int prePos = GetBoardPosition(x, preY);
                    if (GameBoard[prePos] == tileType)
                    {
                        Tile tile = GetTile(x, preY);
                        if (tile != null)
                        {
                            comboList.Add(tile);
                            preY -= 1;
                        }
                    }
                    else
                    {
                        preY = -1;
                    }
                }

                if (postY < BoardHeight)
                {
                    int postPos = GetBoardPosition(x, postY);

                    if (GameBoard[postPos] == tileType)
                    {
                        Tile tile = GetTile(x, postY);
                        if (tile!= null)
                        {
                            comboList.Add(tile);
                            postY += 1;
                        }
                    }
                    else
                    {
                        postY = BoardHeight;
                    }
                }
            }

            return comboList;
        }

        private Tile GetTile(int x, int y)
        {
            Collider2D tileHit =  Physics2D.OverlapPoint(new Vector2(transform.position.x + x, transform.position.y - y));
            if (tileHit != null && tileHit.tag == "Tile") return tileHit.GetComponent<Tile>();

            return null;

        }
    }
}