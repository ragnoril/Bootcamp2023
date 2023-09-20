using ShooterGame;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Rigidbody _rigidBody;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private PlayerController _target;

    [SerializeField]
    private float _maxHealth;

    private float _currentHealth;


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _target = GameObject.Find("Player").GetComponent<PlayerController>();

        _currentHealth = _maxHealth;
    }

    private void FixedUpdate()
    {
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
                Destroy(gameObject);
        }
    }


}
