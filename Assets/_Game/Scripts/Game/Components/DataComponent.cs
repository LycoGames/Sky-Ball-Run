using System.IO;
using _Game.Scripts.Base.Component;
using _Game.Scripts.Game.Data;
using UnityEngine;

namespace _Game.Scripts.Game.Components
{
    public class DataComponent : MonoBehaviour, IComponent
    {
        private const string LevelDataFileName = "/LevelData.json";
        private const string InventoryDataFileName = "/InventoryData.json";

        public LevelData LevelData => levelData;
        public InventoryData InventoryData => inventoryData;

        private string dataPath;

        private LevelData levelData;
        private InventoryData inventoryData;

        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");

            SetDataPath();

            CreateLevelData();
            CreateInventoryData();


            SaveLevelData();
            SaveInventoryData();
        }

        public void SaveLevelData()
        {
            SaveData(LevelDataFileName, in levelData);
        }

        public void SaveInventoryData()
        {
            SaveData(InventoryDataFileName, in inventoryData);
        }

        private void SetDataPath()
        {
#if UNITY_EDITOR
            dataPath = Application.dataPath;
#else
            dataPath = Application.persistentDataPath;
#endif
        }

        private void LoadData<T>(string dataFileName, out T dataObject)
        {
            string content = File.ReadAllText(dataPath + dataFileName);
            dataObject = JsonUtility.FromJson<T>(content);
        }

        private void SaveData<T>(string dataFileName, in T dataObject)
        {
            string content = JsonUtility.ToJson(dataObject);
            File.WriteAllText(dataPath + dataFileName, content);
        }

        private void CreateLevelData()
        {
            if (!File.Exists(dataPath + LevelDataFileName))
                levelData = new LevelData()
                {
                    currentLevel = 0,
                };
            else
                LoadData(LevelDataFileName, out levelData);
        }

        private void CreateInventoryData()
        {
            if (!File.Exists(dataPath + InventoryDataFileName))
                inventoryData = new InventoryData
                {
                    //ownedCoin = 0,
                    ownedDiamond = 0,
                };
            else
                LoadData(InventoryDataFileName, out inventoryData);
        }
    }
}