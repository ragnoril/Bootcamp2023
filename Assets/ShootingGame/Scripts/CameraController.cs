using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class CameraController : MonoBehaviour
    {

        [SerializeField]
        private Transform _target;

        private Vector3 _offsetPosition;

        void Start()
        {
            _offsetPosition = _target.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = _target.position - _offsetPosition;

            transform.LookAt(_target.position);
        }


    }
}