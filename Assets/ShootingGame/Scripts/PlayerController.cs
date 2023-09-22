using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace ShooterGame
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidBody;
        private Animator _animator;

        [SerializeField]
        private float _moveSpeed;

        [SerializeField]
        private Camera _gameCam;

        [SerializeField]
        private GameObject _bulletPrefab;

        [SerializeField]
        private Transform _muzzleTransform;

        [SerializeField]
        private float _rateOfFire;
        private float _fireTimer;

        [SerializeField]
        private float _maxHealth;
        private float _currentHealth;

        private int _score;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidBody = GetComponent<Rigidbody>();
            _fireTimer = _rateOfFire;
            _currentHealth = _maxHealth;
            _score = 0;
        }

        private void Update()
        {
            Ray mouseRay = _gameCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(mouseRay, out hitInfo, 100f, 1 << 3)) 
            {
                float angleBetween = 270f - Mathf.Atan2(transform.position.z - hitInfo.point.z, transform.position.x - hitInfo.point.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angleBetween, 0);
            }

            _fireTimer += Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
                if (_fireTimer > _rateOfFire)
                    Shoot();
            }

        }

        private void Shoot()
        {
            GameObject go = Instantiate(_bulletPrefab, _muzzleTransform.position, _muzzleTransform.rotation);
            _fireTimer = 0f;

        }

        private void FixedUpdate()
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            Vector3 playerVelocity = new Vector3(inputX, 0, inputZ) * _moveSpeed * Time.fixedDeltaTime;
            playerVelocity.y = _rigidBody.velocity.y;

            _rigidBody.velocity = playerVelocity;

            if (_rigidBody.velocity.sqrMagnitude > 0.1f)
                _animator.SetBool("IsWalking", true);
            else
                _animator.SetBool("IsWalking", false);
        }

        public void GotHit(float damage)
        {
            
            _currentHealth -= damage;
            Debug.Log("hit:" + damage + " health: " + _currentHealth);
            if (_currentHealth <= 0)
                _animator.SetBool("IsDead", true);
            else
                _animator.SetTrigger("GotHit");

        }

        public void EnemyKilled()
        {
            _score += 1;
        }

    }
}