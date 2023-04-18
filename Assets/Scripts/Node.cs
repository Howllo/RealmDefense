using Managers;
using TurretScripts;
using UnityEngine;

public class Node : MonoBehaviour
{
    public string objectGuid;
    private GameObject _turret;
    private Renderer _rend;
    private Color _startColor;
    public Color hoverColor;
    public Vector3 positionOffSet;

    private void Awake()
    {
        gameObject.name = objectGuid;
    }

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _startColor = _rend.material.color;
    }
    
    
    public void OnNewMouseDown(TurretType type)
    {
        if (_turret != null)
        {
            print("Can't build there! - TODO: Display red turret.");
            return;
        }

        if (GameManager.Instance.GetPlayerGold() - BuildManager.Instance.GetTurretScript(type).GetCostToBuild() < 0)
        {
            print("You do not have enough money. TODO: UI element to show red.");
            return;
        }
        
        //Build a turret
        _turret = Instantiate(BuildManager.Instance.GetTurretToBuild(TurretType.Prototype), transform.position + positionOffSet, transform.rotation);
    }

    public void OnNewMouseEnter()
    {
        _rend.material.color = hoverColor;
    }

    public void OnNewMouseExit()
    {
        _rend.material.color = _startColor;
    }

    public GameObject GetTurret()
    {
        return _turret;
    }
}
