using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Platformer
{
    public class PlayerController : CustomBehaviour
    {
        private PlayerData _data;
        public PlayerData Data { get { return _data; } }
        private PlayerAgent _agent;

        [SerializeField]
        private Rigidbody2D _rigidBody;

        [SerializeField] 
        private AudioClip _pickUpClip;

        [SerializeField]
        private Transform _groundCheck;

        [SerializeField]
        private bool _isGrounded;

        [SerializeField]
        private bool _isPaused;


        private NewControls _controller;

        private float _playerMoveDirection;



        private void Start()
        {
            _data = GetComponent<PlayerData>();
            _agent = GetComponent<PlayerAgent>();
            
            HandleControls();

            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody2D>();
            }

            if (_groundCheck == null)
            {
                _groundCheck = transform.Find("TouchGround");
            }

            _isPaused = false;
        }

        private void HandleControls()
        {
            _controller = new NewControls();
            _controller.Enable();
            _controller.PlayerControls.Move.performed += MovePlayerInput;
            _controller.PlayerControls.Move.canceled += ClearMoveInput;
            _controller.PlayerControls.Jump.performed += JumpPlayerInput;
        }

        private void ClearMoveInput(InputAction.CallbackContext obj)
        {
            _playerMoveDirection = 0f;
        }

        private void JumpPlayerInput(InputAction.CallbackContext input)
        {
            Jump();
        }

        private void Jump()
        {
            Debug.Log("Jump");
            if (_isGrounded)
            {
                _rigidBody.AddForce(Vector2.up * _data.JumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
                _isGrounded = false;
                _agent.Jump();
            }
        }

        private void MovePlayerInput(InputAction.CallbackContext input)
        {
            _playerMoveDirection = input.ReadValue<float>();
        }

        public override void Init(GameManager gameManager)
        {
            base.Init(gameManager);

            _gameManager.OnLevelStarted += StartNewLevel;
            _gameManager.OnLevelCompleted += LevelFinished;
        }

        private void OnDestroy()
        {
            _gameManager.OnLevelStarted -= StartNewLevel;
            _gameManager.OnLevelCompleted -= LevelFinished;
        }

        private void LevelFinished()
        {
            _isPaused = true;
        }

        private void StartNewLevel()
        {
            if (_data == null)
                _data = GetComponent<PlayerData>();
            _data.CoinsCollected = 0;
            transform.position = Vector3.zero;
            if (_agent == null)
                _agent = GetComponent<PlayerAgent>();
            _agent.StopAnimations();
            _isPaused = false;
        }

        private void FixedUpdate()
        {
            if (_isPaused) return;

            int layerMask = LayerMask.GetMask("Floor");
            _isGrounded = Physics2D.OverlapPoint(_groundCheck.position, layerMask);

            //float moveX = Input.GetAxis("Horizontal");

            Vector2 newVelocity = new Vector2(_playerMoveDirection * _data.MoveSpeed * Time.fixedDeltaTime, _rigidBody.velocity.y);
            _rigidBody.velocity = newVelocity;

            _agent.Move(_rigidBody.velocity);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.tag == "Coin")
            {
                _data.CoinsCollected += collision.GetComponent<PickUp>().GetPickUp();
                _gameManager.Audio.PlaySound(_pickUpClip);
            }
            else if (collision.tag == "Finish")
            {
                _gameManager.CheckIfLevelEnded();
            }
        }
    }

}