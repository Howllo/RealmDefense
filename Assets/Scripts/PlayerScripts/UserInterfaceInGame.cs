using Managers;
using TMPro;
using UnityEngine;

namespace PlayerScripts
{
    /// <summary>
    /// For all user interface needs within the game levels.
    /// </summary>
    public class UserInterfaceInGame : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI heartText;
        [SerializeField] private TextMeshProUGUI goldText;

        private void Start()
        {
            GameManager.Instance.updateHeartUserInterface += UpdateHeart;
            GameManager.Instance.updateGoldUserInterface += UpdateGold;
        }

        private void UpdateHeart(int heart)
        {
            heartText.SetText(heart.ToString());
        }
        
        private void UpdateGold(int gold)
        {
            goldText.SetText(gold.ToString());
        }
    }
}