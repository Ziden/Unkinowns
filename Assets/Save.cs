using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class Save {

	/// <summary>
	/// STORED PART
	/// </summary>
    public List<int> levelsFinished = new List<int>();

    public static Save save = new Save();

    public static void saveGame()
    {

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/save.gd");
        bf.Serialize(file, save);
        file.Close();
    }

    public static void loadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/save.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/save.gd", FileMode.Open);
            save = (Save)bf.Deserialize(file);
            file.Close();
        }
    }

}
