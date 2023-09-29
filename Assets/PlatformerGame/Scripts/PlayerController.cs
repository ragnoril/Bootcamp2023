using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerData _data;
        private PlayerAgent _agent;

        [SerializeField]
        private Rigidbody2D _rigidBody;

        [SerializeField] 
        private AudioClip _pickUpClip;

        [SerializeField]
        private Transform _groundCheck;

        [SerializeField]
        private bool _isGrounded;


        private void Start()
        {
            _data = GetComponent<PlayerData>();
            _agent = GetComponent<PlayerAgent>();

            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody2D>();
            }

            if (_groundCheck == null)
            {
                _groundCheck = transform.Find("TouchGround");
            }

        }

        private void FixedUpdate()
        {
            int layerMask = LayerMask.GetMask("Floor");
            _isGrounded = Physics2D.OverlapPoint(_groundCheck.position, layerMask);

            float moveX = Input.GetAxis("Horizontal");

            if (_isGrounded && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                _rigidBody.AddForce(Vector2.up * _data.JumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
                _isGrounded = false;
                _agent.Jump();
            }

            Vector2 newVelocity = new Vector2(moveX * _data.MoveSpeed * Time.fixedDeltaTime, _rigidBody.velocity.y);
            _rigidBody.velocity = newVelocity;

            _agent.Move(_rigidBody.velocity);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.tag == "Coin")
            {
                _data.CoinsCollected += collision.GetComponent<PickUp>().GetPickUp();
                AudioManager.Instance.PlaySound(_pickUpClip);
            }
        }
    }

}