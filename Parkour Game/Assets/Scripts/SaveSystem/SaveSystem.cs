using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    string filePath;
    string saveFileName;

    public static SaveSystem instance;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/" + saveFileName + "/save.data";

        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SaveGame(GameData saveData)
    {
        FileStream dataStream = new FileStream(filePath, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();

        // Convert the data and sen it to the file
        converter.Serialize(dataStream, saveData);

        dataStream.Close();
    }

    public GameData LoadGame()
    {
        if (!File.Exists(filePath)) return null;

        // Check if file already exists
        // If the file exists, get the existing file and return it
        FileStream dataStream = new FileStream(filePath, FileMode.Open);

        BinaryFormatter converter = new BinaryFormatter();
        GameData saveData = converter.Deserialize(dataStream) as GameData;

        dataStream.Close();
        return saveData;

           
        // If the file does not exist, return an error message and cancle the function

    }
}
