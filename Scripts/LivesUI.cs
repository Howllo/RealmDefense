using TMPro;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    public TextMeshProUGUI livesText;
    
    void FixedUpdate()
    {
        livesText.SetText(PlayerStats.lives.ToString());
    }
}
