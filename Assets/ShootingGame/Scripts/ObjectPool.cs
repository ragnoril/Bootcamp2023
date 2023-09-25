using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


namespace ShooterGame
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        public IObjectPool<BulletController> BulletPool;
        public BulletController BulletPrefab;

        public IObjectPool<EnemyController> EnemyPool;
        public EnemyController EnemyPrefab;

        private void Awake()
        {
            if (Instance != null && Instance == this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                Instance = this;
            }

            BulletPool = new ObjectPool<BulletController>(CreateBullet, GetBullet, ReleaseBullet, DestroyBullet);
            EnemyPool = new ObjectPool<EnemyController>(CreateEnemy, GetEnemy, ReleaseEnemy, DestroyEnemy);
        }


        private void DestroyBullet(BulletController obj)
        {
            Destroy(obj.gameObject);
        }

        private void ReleaseBullet(BulletController obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void GetBullet(BulletController obj)
        {
            obj.gameObject.SetActive(true);
        }

        private BulletController CreateBullet()
        {
            BulletController bullet = Instantiate(BulletPrefab);
            bullet.transform.SetParent(transform);

            return bullet;
        }

        private EnemyController CreateEnemy()
        {
            EnemyController enemy = Instantiate(EnemyPrefab);
            enemy.transform.SetParent(transform);

            return enemy;
        }

        private void GetEnemy(EnemyController obj)
        {
            obj.gameObject.SetActive(true);
        }

        private void ReleaseEnemy(EnemyController obj)
        {
            obj.gameObject.SetActive(false);
        }

        private void DestroyEnemy(EnemyController obj)
        {
            Destroy(obj.gameObject);
        }


        /*
        public List<GameObject> PooledObjects;
        public GameObject ObjectToPool;
        public int PoolLimit;

        private void Start()
        {
            PooledObjects = new List<GameObject>();
            for(int i = 0; i < PoolLimit; i++)
            {
                GameObject go = Instantiate(ObjectToPool);
                go.SetActive(false);
                go.transform.SetParent(transform);
                PooledObjects.Add(go);
            }

        }

        public GameObject GetPooledObject()
        {
            for(int i = 0; i< PoolLimit; i++)
            {
                if (!PooledObjects[i].activeSelf)
                    return PooledObjects[i];
            }

            return null;
        }

        */

    }
}