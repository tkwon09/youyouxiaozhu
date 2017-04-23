using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Text.RegularExpressions;

/// At the beginning of the game, load resources of the game first.
/// Then load the saved data and create respective object and attach
/// corresponding script.
/// This is a class holding necessary resources for the game
public class DataManager : MonoBehaviour {

    public GameObject Player;
    Attributes playerAttr;
    
    PlayerData loadeddata = new PlayerData();

    Dictionary<string, Dictionary<int, int[]>> IKFLevelPlus = new Dictionary<string, Dictionary<int, int[]>>();
    Dictionary<string, string> IKFDesc = new Dictionary<string, string>();

	// Use this for initialization
	void Start ()
    {
        playerAttr = Player.GetComponent<Attributes>();
        
        LoadResources();
        LoadData();
        //SaveData();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    [Serializable]
    class PlayerData
    {
        public string ikfName;
        public int ikfLevel;
        public int KP;
        public int fame;
        public int dressing;
    }

    void SaveData()
    {
        InnerKF playerIKF;
        playerIKF = Player.GetComponent<InnerKF>();

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerData.dat");

        PlayerData data = new PlayerData();
        data.ikfName = playerIKF.ikfName;
        data.ikfLevel = playerIKF.level;
        int[] attr = playerAttr.attrSave();
        data.KP = attr[0];
        data.fame = attr[1];
        data.dressing = attr[2];

        bf.Serialize(file, data);
        file.Close();
    }

    void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/playerData.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerData.dat", FileMode.Open);
            loadeddata = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Load attributes
            int[] attr = new int[3];
            attr[0] = loadeddata.KP;
            attr[1] = loadeddata.fame;
            attr[2] = loadeddata.dressing;
            playerAttr.attrLoad(attr);
            // Load player's innerKF
            AddIKF(loadeddata.ikfName, loadeddata.ikfLevel);
        }
        else
        {
            // First time, start a new data
            // Load attributes
            int[] attr = new int[3];
            attr[0] = 0;
            attr[1] = 0;
            attr[2] = 0;
            playerAttr.attrLoad(attr);
            playerAttr.attrChange(0, Attributes.initialHealth);
            playerAttr.attrChange(1, Attributes.initialChi);
            playerAttr.attrChange(2, Attributes.initialStamina);
            playerAttr.attrChange(3, 0);
        }
    }

    void LoadResources()
    {

        //playerIKF.levelPlus;
        LoadIKFLevelPlus();
        LoadIKFDesc();
    }
    void AddIKF(string ikfName, int level)
    {
        Player.GetComponent<InnerKF>();
        InnerKF tempIKF = Player.GetComponent<InnerKF>();
        InnerKF.plus tempPlus = new InnerKF.plus();
        tempPlus.healthPlus = IKFLevelPlus[ikfName][level][0];
        tempPlus.chiPlus = IKFLevelPlus[ikfName][level][1];
        tempPlus.IPPlus = IKFLevelPlus[ikfName][level][2];
        tempPlus.staminaPlus = IKFLevelPlus[ikfName][level][3];
        tempIKF.levelPlus = tempPlus;
        tempIKF.ikfName = ikfName;
        tempIKF.level = level;
        tempIKF.desc = IKFDesc[ikfName];
        // Let attributes load IKF plus
        playerAttr.attrLoadIKF();
        // Add corresponding ikf buff
        playerAttr.AddBuff(ikfName, false);
    }
    void LoadIKFLevelPlus()
    {
        TextAsset textFile = Resources.Load<TextAsset>("Text/IKFLevelPlus");
        string[] lines = textFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        string mainPattern = @"(\w+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+)";
        string subPattern = @"(\d+).(\d+).(\d+).(\d+)";
        foreach (string s in lines)
        {
            foreach (Match m in Regex.Matches(s, mainPattern))
            {
                string kfName = m.Groups[1].Value;
                IKFLevelPlus[kfName] = new Dictionary<int, int[]>();
                for (int i = 2; i <= 10; i++)
                {
                    string temp = m.Groups[i].Value;
                    foreach (Match n in Regex.Matches(temp, subPattern))
                    {
                        int[] tempArr = new int[4];
                        for (int j = 1; j <= 4; j++)
                            tempArr[j - 1] = Int32.Parse(n.Groups[j].Value);
                        IKFLevelPlus[kfName][i - 1] = new int[4];
                        IKFLevelPlus[kfName][i - 1] = tempArr;
                    }
                }
            }
        }
    }
    void LoadIKFDesc()
    {
        TextAsset textFile = Resources.Load<TextAsset>("Text/IKFDesc");
        string[] lines = textFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        string mainPattern = @"(\w+).([\w\s]+)";
        foreach (string s in lines)
        {
            foreach (Match m in Regex.Matches(s, mainPattern))
            {
                IKFDesc[m.Groups[1].Value] = m.Groups[2].Value;
            }
        }
    }
}
