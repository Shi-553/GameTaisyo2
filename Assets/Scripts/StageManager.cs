using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Scene;
using System.IO;

public class StageManager : SingletonMonoBehaviour<StageManager> {

    [SerializeField]
    int debugStage = 1;
    int stage = 0;

    SaveDataWrapper wrapper = null;
    string SAVEDATA_PATH;
    string STAGE_PATH;

    override protected void  Awake() {
        base.Awake();
        SAVEDATA_PATH = Path.Combine(Application.dataPath, "SaveData.json");
        STAGE_PATH = Path.Combine(Application.dataPath, "Resources/Stage");

        if (!LoadJson()) {
            SaveJson();
        }

        DontDestroyOnLoad(gameObject);
    }
    bool LoadJson() {
        if (!File.Exists(SAVEDATA_PATH)) {
            return false;
        }
        var savadataTextAsset = File.ReadAllText(SAVEDATA_PATH);
        if (savadataTextAsset == "") {
            return false;
        }

        var savadataString = savadataTextAsset.ToString();
        wrapper = JsonUtility.FromJson<SaveDataWrapper>(savadataString);
        if (wrapper == null) {
            return false;
        }
        return true;
    }
    void SaveJson() {
        if (wrapper == null) {
            wrapper = new SaveDataWrapper();
            wrapper.saveDatas = new List<SaveData>();
            for (int i = 0; i < Directory.GetFiles(STAGE_PATH, "*.prefab").Length; i++) {
                wrapper.saveDatas.Add(new SaveData(StageStatus.UNLOCK));
            }
        }
        var savedataString = JsonUtility.ToJson(wrapper);
        File.WriteAllText(SAVEDATA_PATH, savedataString);
    }
    public void ClearStage(StageStatus status) {
        GetData(stage).status = status;
        SaveJson();
    }

    public void LoadStage() {
        if (stage == 0) {
            stage = debugStage;
        }
        var stagePrefab = Resources.Load<GameObject>(Path.Combine("Stage/", stage.ToString()));

        if (stagePrefab != null) {
            var stageInstance = Instantiate<GameObject>(stagePrefab);

            stageInstance.transform.SetParent(GameObject.Find("stage").transform);
        }
    }
    public bool NextStage() {
        stage++;
        return GetData(stage) != null;
    }
    public void SetStage(int s) {
        stage = s ;
    }

    public SaveData GetData(int s) {
        if (s - 1 < 0 || wrapper.saveDatas.Count <= s-1) {
            return null;
        }
        return wrapper.saveDatas[s-1];
    }

}
[Serializable]
public enum StageStatus {
    LOCK = 0,
    UNLOCK,
    CLEAR,
    NO_DAMAGE,
    PURE_NO_DAMAGE
}
[Serializable]
public class SaveData {
    public StageStatus status;

    public SaveData(StageStatus status) {
        this.status = status;
    }
}
[Serializable]
public class SaveDataWrapper {
    public List<SaveData> saveDatas;
}
