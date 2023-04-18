using UnityEngine;

namespace Managers
{
    public enum GameManagerEnum
    {
        Add,
        Subtract
    }
    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        // Event Update Money
        public delegate void UpdateGoldUserInterface(int gold);
        public UpdateGoldUserInterface updateGoldUserInterface;
        
        // Event Update Hearts
        public delegate void UpdateHeartUserInterface(int heart);
        public UpdateHeartUserInterface updateHeartUserInterface;

        [Header("Information Tracker")] 
        [SerializeField] private int currentHeart;
        [SerializeField] private int currentGold;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else 
                Destroy(gameObject);

        }
        
        private void Start()
        {
            updateHeartUserInterface?.Invoke(currentHeart);
            updateGoldUserInterface?.Invoke(currentGold);
        }

        public int GetPlayerHeart()
        {
            return currentHeart;
        }

        /// <summary>
        /// Used to update hearts in the game.
        /// </summary>
        /// <param name="heart">Takes in a amount to be subtracted from current hearts.</param>
        public void SetPlayerHeart(int heart)
        {
            currentHeart -= heart;
            updateHeartUserInterface?.Invoke(currentHeart);
        }

        public int GetPlayerGold()
        {
            return currentGold;
        }

        /// <summary>
        /// Used to update gold in the game.
        /// </summary>
        /// <param name="gold">Takes in a amount to be subtracted from current gold.</param>
        public void SetPlayerGold(int gold, GameManagerEnum type)
        {
            if (type == GameManagerEnum.Add)
            {
                currentGold += gold;
            } 
            else if (type == GameManagerEnum.Subtract)
            {
                currentGold -= gold;
            }
            updateGoldUserInterface?.Invoke(currentGold);
        }
    }
}
