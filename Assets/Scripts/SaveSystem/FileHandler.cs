using System;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public abstract class FileHandler
    {
        private string _filePath;
        private string _fileName;

        public FileHandler(string path, string name)
        {
            _filePath = path;
            _fileName = name;
        }

        public PlayerSaveData LoadPlayer()
        {
            string path = Path.Combine(_filePath, _fileName);
            PlayerSaveData loadData = null;
            if (File.Exists(path))
            {
                try
                {
                    string dataLoad = "";
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataLoad = reader.ReadToEnd();
                        }
                    }

                    loadData = JsonUtility.FromJson<PlayerSaveData>(dataLoad);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return loadData;
        }

        public void SavePlayer(PlayerSaveData data)
        {
            string path = Path.Combine(_filePath, _fileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
                string serializeData = JsonUtility.ToJson(data, true);
                using (FileStream stream = new FileStream(serializeData, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(serializeData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        public GameSaveData LoadGame()
        {
            string path = Path.Combine(_filePath, _fileName);
            GameSaveData loadData = null;
            if (File.Exists(path))
            {
                try
                {
                    string dataLoad = "";
                    using (FileStream stream = new FileStream(path, FileMode.Open))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            dataLoad = reader.ReadToEnd();
                        }
                    }

                    loadData = JsonUtility.FromJson<GameSaveData>(dataLoad);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            return loadData;
        }

        public void SaveGame(GameSaveData data)
        {
            string path = Path.Combine(_filePath, _fileName);

            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty);
                string serializeData = JsonUtility.ToJson(data, true);
                using (FileStream stream = new FileStream(serializeData, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(serializeData);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        
        
        public void DeleteFile()
        {
            string path = Path.Combine(_filePath, _fileName);
            if(File.Exists(path))
                File.Delete(path);
        }
        
        public bool CheckForNewGame()
        {
            string path = Path.Combine(_filePath, _fileName);
            if (!File.Exists(path))
                return true;
            return false;
        }
    }
}