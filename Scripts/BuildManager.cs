using UnityEngine;

public class BuildManager : MonoBehaviour
{
    //Singleton pattern
    public static BuildManager instance;
    public GameObject focusNode;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }
        instance = this;

    }

    private TurretBlueprint turretToBuild;
    private Node selectedNode;
    public NodeUiScript nodeUI;

    public bool CanBuild { get { return turretToBuild != null; } }

    public bool hasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    public void SelectNode(Node node)
    {
        if(selectedNode == node)
        {
            deselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;
        nodeUI.setTarget(node);
    }

    public void deselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public void SelectTurretToBuild(TurretBlueprint turret, GameObject focusEffect)
    {
        turretToBuild = turret;
        focusNode = focusEffect;
        deselectNode();
    }

    public TurretBlueprint getTurretToBuild()
    {
        return turretToBuild;
    }

    public void DisableFocus()
    {
        focusNode.SetActive(false);
    }
}
