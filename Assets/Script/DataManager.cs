using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Text.RegularExpressions;

public class DataManager : MonoBehaviour {

    Attributes playerAttr;
    InnerKF playerIKF;

    Dictionary<string, Dictionary<int, int[]>> allIKF = new Dictionary<string, Dictionary<int, int[]>>();

	// Use this for initialization
	void Start ()
    {
        GameObject Player = GameObject.Find("Player");
        playerAttr = Player.GetComponent<Attributes>();
        playerIKF = Player.GetComponent<InnerKF>();
        LoadResources();

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
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            playerIKF.ikfName = data.ikfName;
            data.ikfLevel = playerIKF.level;
            int[] attr = new int[3];
            attr[0] = data.KP;
            attr[1] = data.fame;
            attr[2] = data.dressing;
            playerAttr.attrLoad(attr);
            
        }
    }
    public int[] QianKunDaNuoYi = new int[36];

    void LoadResources()
    {

        //playerIKF.levelPlus;
        TextAsset jsonFile = Resources.Load<TextAsset>("IKFList");
        string[] lines = jsonFile.text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        string mainPattern = @"(\w+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+).([\d,]+)";
        string subPattern = @"(\d+).(\d+).(\d+).(\d+)";
        foreach(string s in lines)
        {
            foreach(Match m in Regex.Matches(s,mainPattern))
            {
                string kfName = m.Groups[1].Value;
                allIKF[kfName] = new Dictionary<int, int[]>();
                for (int i = 2;i <= 10;i++)
                {
                    string temp = m.Groups[i].Value;
                    foreach(Match n in Regex.Matches(temp,subPattern))
                    {
                        int[] tempArr = new int[4];
                        for (int j = 1; j <= 4; j++)
                            tempArr[j - 1] = Int32.Parse(n.Groups[j].Value);
                        allIKF[kfName][i - 1] = new int[4];
                        allIKF[kfName][i - 1] = tempArr;
                    }
                }
            }
        }
    }

}
