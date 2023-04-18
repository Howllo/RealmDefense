namespace SaveSystem
{
    public interface IDataPersistence
    {
        public void LoadData(PlayerSaveData data);
        public void SaveData(ref PlayerSaveData data);

        public void LoadData(GameSaveData data);
        public void SaveData(ref GameSaveData data);
    }
}