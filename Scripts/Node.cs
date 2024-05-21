using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Color hoverColor;
    public Color notEnoughMoneyColor;
    public Vector3 positionOffSet;


    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    
    [HideInInspector]
    public bool isUpgraded = false;
    private Renderer rend;
    private Color startColor;
    BuildManager buildManager;


    private void Start()
    {
        rend = GetComponent<Renderer>();
        if(rend.materials.Length > 1)
        {
            startColor = rend.materials[1].color;
        }
        else
        {
            startColor = rend.material.color;
        }

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffSet;
    }

    void OnMouseDown()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }

        //Build a turret
        buildTurret(buildManager.getTurretToBuild());
    }

    void buildTurret(TurretBlueprint blueprint)
    {
        
        if (PlayerStats.Money < blueprint.cost)
        {
            Debug.Log("Not enough money.");
            return;
        }
        
        PlayerStats.Money -= blueprint.cost;
        GameObject _turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        turretBlueprint = blueprint;

        Debug.Log("Turret built!");
    }

    public void upgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        //Get rid of old turret
        Destroy(turret);

        //Build a new turret
        GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;
        isUpgraded = true;
        Debug.Log("Turret upgraded!");
    }

    public void sellTurret()
    {
        PlayerStats.Money += turretBlueprint.getSellAmount();

        Destroy(turret);
        turretBlueprint = null;
    }


    void OnMouseEnter()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (!buildManager.CanBuild)
        {
            return;
        }
        
        if(buildManager.hasMoney)
        {
            rend.materials[1].color = hoverColor;
        }
        else
        {
            rend.materials[1].color = notEnoughMoneyColor;
        }

    }

    void OnMouseExit()
    {
        rend.materials[1].color = startColor;
    }
}
