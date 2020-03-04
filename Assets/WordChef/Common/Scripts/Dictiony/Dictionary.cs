using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Newtonsoft.Json;



public class Dictionary : MonoBehaviour
{
   
    public void GetData()
    {
        Debug.Log("hdasdasd");
        var client = new WebClient();
        var text = client.DownloadString("https://api.tracau.vn/WBBcwnwQpV89/s/hello/en");
        //var json= JsonConvert.SerializeObject(text);
        //Debug.Log(json);
        
    }

    public class DataDictionary
    {
        public string languague;
        public List<Sentence> senteneces;
        
    } 
    
    public class Sentence
    {
        List<Field> fields;
    }
    
    public class Field
    {
        string en;
        string vi;
    }
}
