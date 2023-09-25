using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;

        [SerializeField] private float _speed;

        // Start is called before the first frame update
        void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        private void FixedUpdate()
        {
            _rigidBody.velocity = transform.forward * _speed * Time.fixedDeltaTime;
            _rigidBody.angularVelocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Enemy")
            {
                GameManager.Instance.EnemyHit(collision.collider.GetComponent<EnemyController>());
            }


            ObjectPool.Instance.BulletPool.Release(this);
        }

    }
}