using System.IO;
using Base.Component;
using UnityEngine;

namespace Game.Components
{
    public class DataComponent : MonoBehaviour, IComponent
    {
        private const string LevelDataFileName = "/LevelData.json";

        private string dataPath;


        public void Initialize(ComponentContainer componentContainer)
        {
            Debug.Log("<color=lime>" + gameObject.name + " initialized!</color>");

            SetDataPath();
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
    }
}