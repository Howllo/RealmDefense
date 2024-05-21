using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    
    void Update()
    {
        moneyText.SetText(PlayerStats.Money.ToString());
    }
}
