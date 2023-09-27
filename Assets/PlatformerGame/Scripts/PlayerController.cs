using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D _rigidBody;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField] 
        private AudioClip _pickUpClip;

        [SerializeField]
        private Transform _groundCheck;

        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private float _jumpSpeed;

        [SerializeField]
        private bool _isGrounded;

        [SerializeField]
        private int _coinsCollected;


        private void Start()
        {
            if (_rigidBody == null)
            {
                _rigidBody = GetComponent<Rigidbody2D>();
            }

            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }

            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (_groundCheck == null)
            {
                _groundCheck = transform.Find("TouchGround");
            }

            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }

            _coinsCollected = 0;
        }

        private void FixedUpdate()
        {
            int layerMask = LayerMask.GetMask("Floor");
            _isGrounded = Physics2D.OverlapPoint(_groundCheck.position, layerMask);

            float moveX = Input.GetAxis("Horizontal");

            if (moveX < 0f)
                _spriteRenderer.flipX = true;
            else
                _spriteRenderer.flipX = false;

            if (_isGrounded && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
            {
                _rigidBody.AddForce(Vector2.up * _jumpSpeed * Time.fixedDeltaTime, ForceMode2D.Impulse);
                _isGrounded = false;
                _animator.SetTrigger("Jump");
            }

            Vector2 newVelocity = new Vector2(moveX * _moveSpeed * Time.fixedDeltaTime, _rigidBody.velocity.y);
            _rigidBody.velocity = newVelocity;

            if (_rigidBody.velocity.y < 0f)
                _animator.SetBool("isFalling", true);
            else
                _animator.SetBool("isFalling", false);

            if (_rigidBody.velocity.x != 0f)
                _animator.SetBool("isWalking", true);
            else
                _animator.SetBool("isWalking", false);

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.tag == "Coin")
            {
                _coinsCollected += collision.GetComponent<PickUp>().GetPickUp();
                _audioSource.PlayOneShot(_pickUpClip);
            }
        }
    }

}