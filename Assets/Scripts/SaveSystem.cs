using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour {
    string filename = "GarbageSeeker.iqs";
    string path;

    private void Update() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.R)) {
            ResetSave();
            Debug.Log("reset Save");
        }
    }

    public void ResetSave() {

        path = Application.persistentDataPath + "/" + filename;
        if (File.Exists(path))
            File.Delete(path);
    }
    public GameData Load() {
        path = Application.persistentDataPath + "/" + filename;
        if (File.Exists(path)) {

            FileStream dataStream = new FileStream(path, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            GameData saveData = converter.Deserialize(dataStream) as GameData;


            dataStream.Close();
            return saveData;
        } else {
            PlayerPrefs.SetInt("sells", 0);
            return new GameData();
        }
    }
    public void Save(GameData saveData) {
        path = Application.persistentDataPath + "/" + filename;
        saveData.lastSaveTime = System.DateTime.Now;

        FileStream dataStream = new FileStream(path, FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, saveData);

        dataStream.Close();
    }
}
