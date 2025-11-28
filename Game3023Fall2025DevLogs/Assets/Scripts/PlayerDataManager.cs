//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using Unity.VisualScripting;
//using UnityEngine;
////using System.IO;

//public class PlayerDataManager : MonoBehaviour
//{
//    public float[] pos;
//    public int score;
//    public int battles;
//    public int wins;
//    public int lost;
//    public int heal;
//    public int nukes;
//    public int shield;

//    public void SaveGame()
//    {
//        PlayerData playerData = new PlayerData();
//        // playerData.pos = new float[] {playerTransform.position.x, playerTransform.position.y, playerTransform.position.z }; 
//        playerData.score = PlayerScore.score;
//        playerData.battles = PlayerScore.battles;
//        playerData.wins = PlayerScore.wins;
//        playerData.lost = PlayerScore.lost;
//        playerData.heal = PlayerScore.heal;
//        playerData.nukes = PlayerScore.nukes;
//        playerData.shield = PlayerScore.shield;

//        string json = JsonUtility.ToJson(playerData);
//        string path = Application.persistentDataPath + "/playerData.json";
//        System.IO.File.WriteAllText(path, json);



//    }

//    public void loadGame()
//    {
//        string path = Application.persistentDataPath + "/playerData.json";

//        if (File.Exists(path))
//        {
//            string json = System.IO.File.ReadAllText(path);
//            PlayerData LoadedData = JsonUtility.FromJson<PlayerData>(json);


//            PlayerScore.score = LoadedData.score;
//            PlayerScore.battles = LoadedData.battles;
//            PlayerScore.wins = LoadedData.wins;
//            PlayerScore.lost = LoadedData.lost;
//            PlayerScore.heal = LoadedData.heal;
//            PlayerScore.nukes = LoadedData.nukes;
//            PlayerScore.shield = LoadedData.shield;

//        }
//        else
//        {
//            Debug.LogWarning("File not found!");
//        }
//    }
//}