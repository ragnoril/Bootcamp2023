using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterGame
{
    public class MousePointer : MonoBehaviour
    {

        private Camera _gameCam;

        private void Start()
        {
            _gameCam = Camera.main;
        }

        void Update()
        {
            /*
            Vector3 targetPos = _gameCam.ScreenToWorldPoint(Input.mousePosition + (Vector3.forward * _gameCam.transform.position.y));
            targetPos.y = 0.01f;

            transform.position = targetPos;
            */

            Ray mouseRay = _gameCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(mouseRay, out hitInfo, 100f, 1 << 3))
            {
                /*
                float angleBetween = 270f - Mathf.Atan2(transform.position.z - hitInfo.point.z, transform.position.x - hitInfo.point.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, angleBetween, 0);
                */
                Vector3 pos = hitInfo.point;
                pos.y = 0.01f;

                transform.position = pos;
            }
        }
    }
}