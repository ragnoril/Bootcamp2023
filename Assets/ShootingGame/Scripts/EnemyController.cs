using ShooterGame;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody _rigidBody;
    private Animator _animator;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private PlayerController _target;

    [SerializeField]
    private float _maxHealth;

    private float _currentHealth;

    [SerializeField]
    private float _attackRate;
    private float _attackTimer;

    private bool _canAttack;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();
        _target = GameObject.Find("Player").GetComponent<PlayerController>();

        _currentHealth = _maxHealth;
        _attackTimer = 0f;

        _canAttack = false;
    }

    private void Update()
    {
        _attackTimer += Time.deltaTime;

        if ( _canAttack && _attackTimer > _attackRate)
        {
            Attack();
        }
    }

    private void FixedUpdate()
    {
        if (_canAttack)
        {
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
            return;
        }

        float angleBetween = 270f - Mathf.Atan2(transform.position.z - _target.transform.position.z, transform.position.x - _target.transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, angleBetween, 0);

        _rigidBody.velocity = transform.forward * _moveSpeed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet")
        {
            _currentHealth -= 1;

            if (_currentHealth <= 0)
            {
                _animator.SetBool("IsDead", true);
                _target.EnemyKilled();
                Destroy(gameObject, 1f);
            }
            else
            {
                _animator.SetTrigger("GotHit");
            }
        }

        if (collision.collider.tag == "Player")
        {
            _canAttack = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            _canAttack = false;
        }
    }

    private void Attack()
    {
        _attackTimer = 0f;
        _animator.SetTrigger("Attack");
        _target.GotHit(1);
    }
}
