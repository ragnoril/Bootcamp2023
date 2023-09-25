using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

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
        private int _maxHealth;
        private int _currentHealth;

        private int _score;

        [SerializeField]
        private HealthBar _healthBar;


        private void Start()
        {
            _animator = GetComponent<Animator>();
            _rigidBody = GetComponent<Rigidbody>();
            _fireTimer = _rateOfFire;
            _currentHealth = _maxHealth;
            _healthBar.UpdateHealthbar(_maxHealth, _currentHealth);
            _score = 0;

            GameManager.Instance.OnEnemyKilled += EnemyKilled;
            GameManager.Instance.OnPlayerHit += GotHit;
        }

        private void OnDestroy()
        {
            GameManager.Instance.OnEnemyKilled -= EnemyKilled;
            GameManager.Instance.OnPlayerHit -= GotHit;
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
            
            /*
            Vector3 targetPos = _gameCam.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * _gameCam.transform.position.y));
            targetPos.y = transform.position.y;

            transform.LookAt(targetPos);
            */

            _fireTimer += Time.deltaTime;
            if (Input.GetMouseButton(0))
            {
                if (_fireTimer > _rateOfFire)
                    Shoot();
            }

        }

        private void Shoot()
        {
            //GameObject go = Instantiate(_bulletPrefab, _muzzleTransform.position, _muzzleTransform.rotation);
            /*
            GameObject go = objectPool.GetPooledObject();
            go.transform.position = _muzzleTransform.position;
            go.transform.rotation = _muzzleTransform.rotation;
            go.SetActive(true);
            */

            BulletController bullet = ObjectPool.Instance.BulletPool.Get();
            bullet.transform.position = _muzzleTransform.position;
            bullet.transform.rotation = _muzzleTransform.rotation;

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

        public void GotHit(int damage)
        {
            _currentHealth -= damage;
            _healthBar.UpdateHealthbar(_maxHealth, _currentHealth);
            
            if (_currentHealth <= 0)
                _animator.SetBool("IsDead", true);
            else
                _animator.SetTrigger("GotHit");

            if (_currentHealth <= 0)
                GameManager.Instance.PlayerKilled();

        }

        public void EnemyKilled()
        {
            _score += 1;
            GameManager.Instance.ScoreChanged(_score);
        }



    }
}