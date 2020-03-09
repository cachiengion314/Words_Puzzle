using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;


//namespace DictionaryApi
//{

    public class Dictionary:MonoBehaviour
    {
        public string wordValid;
        public bool includeRelated;
        public string sourceDictionaries;
        public bool useCanonical;
        public bool includeTags;
        public string limit;
        public Text textWordName;
        public Text textMean;
        public static Dictionary instance;

        string keyApi;
        string url;
        public static Dictionary<string, List<WordData>> wordsDict = new Dictionary<string, List<WordData>>();
        List<WordData> listMeanWord = new List<WordData>();
        WordData wordData;


        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {

        }

        public void GetDataFromApi()
        {
            sourceDictionaries = "wiktionary";
            keyApi = "l7bgsd9titsbw82vtpzparyfrmt9yg2hibbeihv5uex8e5maa";
            url = "https://api.wordnik.com/v4/word.json/" + wordValid
                + "/definitions?limit=" + limit
                + "&includeRelated=" + includeRelated
                + "&sourceDictionaries=" + sourceDictionaries
                + "&useCanonical=" + useCanonical
                + "&includeTags=" + includeTags
                + "&api_key=" + keyApi;

            var client = new WebClient();
            var text = client.DownloadString(url);
            JArray arrayJson = JArray.Parse(text);
            for (int i = 0; i < arrayJson.Count; i++)
            {
                wordData = JsonConvert.DeserializeObject<WordData>(arrayJson[i].ToString());
                listMeanWord.Add(wordData);
            }

            wordsDict.Add(wordValid, listMeanWord);
        }

        public void ShowWordData()
        {
            GetDataFromApi();
            foreach (KeyValuePair<string, List<WordData>> word in wordsDict)
            {
                textWordName.text = word.Key;
                foreach (WordData wordMean in word.Value)
                {
                    textMean.text += "(" + wordMean.partOfSpeech + ") " + wordMean.text.Replace("<xref>", "").Replace("</xref>", "") + "\n";
                }
            }
        }


    }
//}
//public class WordData 
//{
//    public string partOfSpeech;
//    public string text;
    
//}

