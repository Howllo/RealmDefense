using System.Collections.Generic;
using TurretScripts;
using UnityEngine;

namespace PlayerScripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private LayerMask turretLayerMask;
        [SerializeField] private LayerMask nodeLayerMask;
        [SerializeField] private LayerMask environmentLayerMask;
        private readonly Dictionary<string, Node> _allNodes = new Dictionary<string, Node>();
        private Camera _cam;
        private string _cacheName;
        private string _lastCache;
        
        private void Start()
        {
            Node[] allNodes = FindObjectsOfType<Node>();
            foreach (var node in allNodes)
                _allNodes.Add(node.gameObject.name, node);
            _cam = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out var hit, 2000.0f, nodeLayerMask))
                {
                    _cacheName= hit.transform.name;
                    if (_allNodes.ContainsKey(_cacheName))
                    {
                        print("TODO: Create a UI popup.");
                        _allNodes[_cacheName].OnNewMouseDown(TurretType.Prototype);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            Ray ray = _cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            
            // Custom On Mouse Enter
            if (Physics.Raycast(ray, out hit, 2000.0f, nodeLayerMask))
            {
                _cacheName = hit.transform.name;
                if (_allNodes.ContainsKey(_cacheName))
                {
                    if (!_allNodes[_cacheName].GetTurret())
                    {
                        _allNodes[_cacheName].OnNewMouseEnter();
                    }
                }
            }

            // Custom On Mouse Exit
            if (Physics.Raycast(ray, out hit))
            {
                if (!string.IsNullOrEmpty(_cacheName) && !string.IsNullOrEmpty(_lastCache))
                {
                    if (_cacheName != _lastCache || hit.transform.CompareTag("Environment"))
                    {
                        if(_allNodes.ContainsKey(_lastCache))
                            _allNodes[_lastCache].OnNewMouseExit();
                    }
                }
            }
            _lastCache = _cacheName;
        }
    }
}
