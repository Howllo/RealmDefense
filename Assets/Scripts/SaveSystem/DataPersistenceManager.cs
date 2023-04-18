using UnityEngine;

namespace SaveSystem
{
    public class DataPersistenceManager : MonoBehaviour
    {
        public static DataPersistenceManager Instance { get; private set; }
        private FileHandler _fileHandler;
        
        private void Awake()
        {
            if(Instance != null)
                Destroy(gameObject);
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }
}