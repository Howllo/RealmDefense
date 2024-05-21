using UnityEngine;
using UnityEngine.UI;

public class NodeUiScript : MonoBehaviour
{
    public GameObject ui;
    public Text upgradeCost;
    public Button upgradeButton;
    public Image upgradeButtonColor;
    public Text sellAmount;
    private Node target;
    public Color startColor;
    public Color notEnoughMoney;
    
    public void setTarget(Node _target)
    {
        target = _target;

        transform.position = new Vector3(target.GetBuildPosition().x, target.GetBuildPosition().y + 3.0f,
            target.GetBuildPosition().z);

        if(!target.isUpgraded)
        {
            if (PlayerStats.Money < _target.turretBlueprint.upgradeCost)
            {
                upgradeButtonColor.color = notEnoughMoney;
                upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
                upgradeButton.interactable = false;
            }
            else
            {
                upgradeButtonColor.color = startColor;
                upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
                upgradeButton.interactable = true;
            }
        }
        else
        {
            upgradeCost.text = "Maxed";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.getSellAmount();

        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
    }

    public void upgrade()
    {
        target.upgradeTurret();
        BuildManager.instance.deselectNode();
    }

    public void sell()
    {
        target.sellTurret();
        BuildManager.instance.deselectNode();
    }

}
