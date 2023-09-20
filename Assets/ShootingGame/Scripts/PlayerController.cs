using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShooterGame
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rigidBody;

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

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _fireTimer = _rateOfFire;
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
        }

    }
}