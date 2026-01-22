using UnityEngine;
using System;
using System.IO;

[Serializable]
public class CropSaveData
{
    public int carrot;
    public int cabbage;
    public int beetroot;
    public int other;
}

public class CropInventory : MonoBehaviour
{
    public static CropInventory Instance;

    public int carrot;
    public int cabbage;
    public int beetroot;
    public int other;

    string SavePath => Path.Combine(Application.persistentDataPath, "crops.json");

    void Awake()
    {
        //ResetInventory();
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Load();
        UICollectible.Instance?.Refresh(carrot, cabbage, beetroot, other);
    }

    public void AddCrop(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            other++;
        }
        else
        {
            switch (id.ToLower())
            {
                case "carrot": carrot++; break;
                case "cabbage": cabbage++; break;
                case "beetroot":
                case "beet": beetroot++; break;
                default:
                    other++;
                    break;
            }
        }

        Save();
        UICollectible.Instance?.Refresh(carrot, cabbage, beetroot, other);
    }

    void Save()
    {
        CropSaveData data = new CropSaveData
        {
            carrot = carrot,
            cabbage = cabbage,
            beetroot = beetroot,
            other = other
        };

        File.WriteAllText(SavePath, JsonUtility.ToJson(data, true));
    }

    void Load()
    {
        if (!File.Exists(SavePath))
        {
            Save();
            return;
        }

        string json = File.ReadAllText(SavePath);
        CropSaveData data = JsonUtility.FromJson<CropSaveData>(json);

        carrot = data.carrot;
        cabbage = data.cabbage;
        beetroot = data.beetroot;
        other = data.other;
    }

    public void ResetInventory()
    {
        carrot = 0;
        cabbage = 0;
        beetroot = 0;
        other = 0;

        Save();
        UICollectible.Instance?.Refresh(carrot, cabbage, beetroot, other);
    }

}
