using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lesson0
{

    public class RollingBall : MonoBehaviour
    {
        public float Speed;
        public float JumpSpeed;
        private Rigidbody rBody;

        private void Start()
        {
            rBody = GetComponent<Rigidbody>();
        }


        void FixedUpdate()
        {
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(x, 0, z) * Time.fixedDeltaTime * Speed;
            movement.y = rBody.velocity.y;

            rBody.velocity = movement;
            //rb.AddForce(movement);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rBody.AddForce(Vector3.up * JumpSpeed, ForceMode.Impulse);
            }

        }




    }
}