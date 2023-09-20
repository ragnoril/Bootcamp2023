using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidBody;

    [SerializeField] private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        
    }

    private void FixedUpdate()
    {
        _rigidBody.velocity = transform.forward * _speed * Time.fixedDeltaTime;
        _rigidBody.angularVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, 0.1f);
    }

}
