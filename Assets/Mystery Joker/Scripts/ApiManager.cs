using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft;
using SimpleJSON;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;

namespace JokerMystery
{
    public class ApiManager : MonoBehaviour
    {

        public TextAsset jsonFiles;
        public JSONNode node;
        //private string foldertPath = "Assets/Mystery Joker/Scripts/JsonFiles";
        public string jsontext;

        public static ApiManager instance;
        //string randomJsonData;
        public bool isDataReceived;
        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }
        /// <summary>
        /// Get Random Json file from a collection of Json files
        /// </summary>
        /// <returns></returns>
       /*public IEnumerator GetRandomJson()
        {
            
            string folderPath = Path.Combine(Application.streamingAssetsPath, "JsonFiles");
            string[] jsonFiles = Directory.GetFiles(folderPath, "*.json");
            print("json files length is " + jsonFiles.Length);
            if (jsonFiles.Length == 0)
            {
                print("no json files");
                yield break;
            }

            int randomIndex = Random.Range(0, jsonFiles.Length);
            string selectedFilePath = jsonFiles[randomIndex];

            // Use UnityWebRequest to load the file on Android
            using (UnityWebRequest www = UnityWebRequest.Get(selectedFilePath))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    string jsonData = www.downloadHandler.text;
                    Debug.Log("Loaded JSON data: " + jsonData);
                    randomJsonData = jsonData;
                    isDataReceived = true;
                }
                else
                {
                    Debug.LogError("Failed to load JSON data: " + www.error);
                }
            }
        }*/

        public string LoadRandomJsonFromResources()
        {
            TextAsset[] jsonFiles = Resources.LoadAll<TextAsset>("JsonFiles");
            Debug.Log("json files length is " + jsonFiles.Length);

            if (jsonFiles.Length == 0)
            {
                Debug.LogError("No JSON files found in Resources/JsonFiles.");
                return null;
            }

            int randomIndex = Random.Range(0, jsonFiles.Length);
            TextAsset selectedFile = jsonFiles[randomIndex];

            return selectedFile.text;
        }

        /// <summary>
        /// Get the Json data from the file and Parse it
        /// </summary>
        /// 

        public void GetGameData()
        {
            StartCoroutine(IeGetGameData());
        }
        public IEnumerator  IeGetGameData()
        {
            /*var json = jsonFiles.ToString();
            Debug.Log("json is " + json);
            var parssedArray = JSON.Parse(json);
            Debug.Log("parssed Array is " + parssedArray);*/
            //StartCoroutine(GetRandomJson());
            string randomJsonData = LoadRandomJsonFromResources();
            //yield return new WaitUntil(() => isDataReceived);
            /*JObject data = JObject.Parse(randomJsonData);*/
            JSONNode data = JSON.Parse(randomJsonData); // parsing Json string
            print("json object " + data.ToString());
            //MyData myData = JsonConvert.DeserializeObject<MyData>(randomJsonData);
            var parssedArray = data;
            print("parssed array is " + parssedArray);
            if (parssedArray != null)
            {
                node = parssedArray["data"];
                print("node is " + node.ToString());
            }
            //print("parssedArray is " + myData.data.itemIndexes);
            yield return null;
        }
      
        /*[System.Serializable]
        public class MyData
        {
            public string status;
            public int statusCode;
            public Data data;
        }

        [System.Serializable]
        public class Data
        {
            public float updatedBalance;
            public float prize;
            public int[][] itemIndexes;
            public bool isFree;
            public float priceUsed;
        }*/

    }
}
