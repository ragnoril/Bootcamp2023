using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3D
{

    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private Transform _pool;

        [SerializeField]
        private Transform _matchingPool;

        [SerializeField]
        private GameObject[] _itemPrefabs;

        [SerializeField]
        private int _coupleCount;

        [SerializeField]
        private int _matchCount;

        [SerializeField]
        private Camera _gameCamera;

        public Camera GameCamera
        {
            get { return _gameCamera; }
        }

        /*
        private SelectableItem _selectedItem;
        private Vector3 _screenPoint;
        private Vector3 _offset;
        */

        public event Action<SelectableItem> OnItemEntersBox;
        public event Action<SelectableItem> OnItemLeavesBox;

        [SerializeField]
        private List<SelectableItem> _itemsInsideBox;


        private void Start()
        {
            if (_gameCamera == null)
            {
                _gameCamera = Camera.main;
            }

            _itemsInsideBox = new List<SelectableItem>();

            FillPool();

            OnItemEntersBox += ItemEntersBox;
            OnItemLeavesBox += ItemLeavesBox;
        }

        private void OnDestroy()
        {
            OnItemEntersBox -= ItemEntersBox;
            OnItemLeavesBox -= ItemLeavesBox;
        }

        private void FillPool()
        {
            for (int i = 0; i < _coupleCount; i++)
            {
                GameObject go1 = Instantiate(_itemPrefabs[i % _itemPrefabs.Length], RandomPositionOverPool(), Quaternion.identity);
                go1.transform.SetParent(transform);
                go1.GetComponent<SelectableItem>().Init(this);

                GameObject go2 = Instantiate(_itemPrefabs[i % _itemPrefabs.Length], RandomPositionOverPool(), Quaternion.identity);
                go2.transform.SetParent(transform);
                go2.GetComponent<SelectableItem>().Init(this);
            }
        }


        private Vector3 RandomPositionOverPool()
        {
            Vector3 position = _pool.position;

            position += new Vector3(UnityEngine.Random.Range(-3.5f, 3.5f), UnityEngine.Random.Range(4f, 7f), UnityEngine.Random.Range(-4f, 6.5f));

            return position;
        }

        /*
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit hitInfo = new RaycastHit();
                Ray mouseRay = _gameCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(mouseRay, out hitInfo))
                {
                    SelectableItem item = hitInfo.collider.GetComponent<SelectableItem>();

                    if (item != null)
                    {
                        _selectedItem = item;
                        _screenPoint = _gameCamera.WorldToScreenPoint(_selectedItem.transform.position);
                        _offset = _selectedItem.transform.position - _gameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _selectedItem = null;
            }

            if (Input.GetMouseButton(0) && _selectedItem != null)
            {
                Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
                Vector3 curPosition = _gameCamera.ScreenToWorldPoint(curScreenPoint) + _offset;
                curPosition.y = 3f;
                _selectedItem.transform.position = curPosition;
            }
            
        }
        */

        public void PlaceItemInsideBox(SelectableItem item)
        {
            OnItemEntersBox?.Invoke(item);
        }

        public void RemoveItemFromBox(SelectableItem item)
        {
            OnItemLeavesBox?.Invoke(item);
        }

        private void ItemLeavesBox(SelectableItem item)
        {
            if (_itemsInsideBox.Contains(item))
                _itemsInsideBox.Remove(item);

            if (_itemsInsideBox.Count == 2)
            {
                CheckIfItemsMatch();
            }
        }

        private void ItemEntersBox(SelectableItem item)
        {
            _itemsInsideBox.Add(item);
            if (_itemsInsideBox.Count == 2)
            {
                CheckIfItemsMatch();
            }
        }

        private void CheckIfItemsMatch()
        {
            if (_itemsInsideBox[0].Type == _itemsInsideBox[1].Type)
            {
                foreach (SelectableItem item in _itemsInsideBox)
                    Destroy(item.gameObject);

                _itemsInsideBox.Clear();

                _matchCount += 1;
            }

        }
    }

}