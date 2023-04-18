using System;
using System.Collections.Generic;
using TurretScripts;
using UnityEngine;

namespace Managers
{
    public class BuildManager : MonoBehaviour
    {
        //Singleton pattern
        public static BuildManager Instance { get; private set; }
        [SerializeField] private GameObject[] turretPrefab;
        private Dictionary<TurretType, GameObject> _getTurret = new Dictionary<TurretType, GameObject>();
        private Dictionary<TurretType, Turret> _getTurretScript = new Dictionary<TurretType, Turret>();

        void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            } else
                Destroy(gameObject);
        }

        private void Start()
        {
            foreach (var turret in turretPrefab)
            {
                Turret tScript = turret.GetComponent<Turret>();
                _getTurret.Add(tScript.GetTurretType(), turret);
                _getTurretScript.Add(tScript.GetTurretType(), tScript);
            }
        }

        public GameObject GetTurretToBuild(TurretType type)
        {
            _getTurret.TryGetValue(type, out var turret);
            return turret;
        }

        public Turret GetTurretScript(TurretType type)
        {
            _getTurretScript.TryGetValue(type, out var turret);
            return turret;
        }
    }
}