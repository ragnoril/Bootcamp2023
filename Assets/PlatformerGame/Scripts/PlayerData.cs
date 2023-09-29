using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{


    public class PlayerData : MonoBehaviour
    {
        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private float _jumpSpeed;

        [SerializeField]
        private int _coinsCollected;


        public float MoveSpeed
        {
            get { return _moveSpeed; }
            set { _moveSpeed = value; }
        }

        public float JumpSpeed
        {
            get { return _jumpSpeed; }
            set { _jumpSpeed = value; }
        }

        public int CoinsCollected
        { 
            get { return _coinsCollected; }
            set { _coinsCollected = value; }
        }

        private void Start()
        {
            _coinsCollected = 0;
        }

    }

}