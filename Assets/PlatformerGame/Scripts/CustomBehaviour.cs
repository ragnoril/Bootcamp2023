
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Platformer
{
    public class CustomBehaviour : MonoBehaviour
    {
        protected GameManager _gameManager;

        public virtual void Init(GameManager gm)
        {
            _gameManager = gm;
        }
    }
}