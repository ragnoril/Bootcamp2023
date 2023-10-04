using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3D
{

    public enum ItemType
    {
        Barrel,
        Burger,
        Carton,
        Cupcake,
        Eggplant,
        OilBottle,
        PepperMill
    }

    public class SelectableItem : MonoBehaviour
    {
        private GameManager _gameManager;
        public ItemType Type;

        private Vector3 _screenPoint;
        private Vector3 _offset;

        private Rigidbody _rigidbody;

        public void Init(GameManager gm)
        {
            _gameManager = gm;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnMouseDown()
        {
            _screenPoint = _gameManager.GameCamera.WorldToScreenPoint(transform.position);
            _offset = transform.position - _gameManager.GameCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z));
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }

        private void OnMouseDrag()
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _screenPoint.z);
            Vector3 curPosition = _gameManager.GameCamera.ScreenToWorldPoint(curScreenPoint) + _offset;
            curPosition.y = 3f;
            transform.position = curPosition;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Matchbox")
            {
                _gameManager.PlaceItemInsideBox(this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Matchbox")
            {
                _gameManager.RemoveItemFromBox(this);
            }
        }

    }

}