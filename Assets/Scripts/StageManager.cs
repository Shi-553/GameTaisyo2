using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Scene;
using System.IO;
using UnityEngine.SceneManagement;

public class StageManager : SingletonMonoBehaviour<StageManager> {

    [SerializeField]
    int debugStage = 1;
    int stage = 0;

    SaveDataWrapper wrapper = null;
    string SAVEDATA_PATH;

    int STAGE_MAX=15;

    int deathCount = 0;
    bool isRelifeRetry = false;
    public void SetRelifeRetry() {
        isRelifeRetry = true;
    }

    override protected void  Awake() {
        base.Awake();
        SAVEDATA_PATH = Path.Combine(Application.dataPath, "SaveData.json");

        if (!LoadJson()) {
            SaveJson();
        }
        if (stage == 0) {
            stage = debugStage;
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
            for (int i = 0; i < STAGE_MAX; i++) {
                wrapper.saveDatas.Add(new SaveData(StageStatus.UNLOCK));
            }
        }
        var savedataString = JsonUtility.ToJson(wrapper);
        File.WriteAllText(SAVEDATA_PATH, savedataString);
    }
    public void ClearStage(StageStatus status) {
        deathCount = 0;
        if (status > GetData(stage).status) {
            GetData(stage).status = status;
        }
        SaveJson();
        Scene.SceneManager.Instance.TimeStop();
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.GAMECLEAR, LoadSceneMode.Additive);
    }
    
    public void GameOverStage() {
        deathCount++;
        Scene.SceneManager.Instance.TimeStop();
        Scene.SceneManager.Instance.ChangeScene(Scene.SceneType.GAMEOVER, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }
    public bool IsDeathHpUp() {
        return deathCount >= 3;
    }
    
    public int GetDeathCount() {
        return deathCount;
    }

    public void LoadStage() {
        var stagePrefab = Resources.Load<GameObject>(Path.Combine("Stage/", stage.ToString()));

        if (stagePrefab != null) {
            var stageInstance = Instantiate<GameObject>(stagePrefab);

            stageInstance.transform.SetParent(GameObject.Find("stage").transform);

            if (IsDeathHpUp()&& isRelifeRetry) {
                Player.PlayerCore.Instance.SetMaxHP(5);
                isRelifeRetry = false;
            }
        }
    }
    public bool NextStage() {
        stage++;
        deathCount = 0;
        return GetData(stage) != null;
    }
    public void SetStage(int s) {
        if (s == stage) {
            return;
        }
        stage = s;
        deathCount = 0;
    }

    
    public int GetStage() {
        return stage;
    }
    public bool IsLastStage { get => stage == wrapper.saveDatas.Count; }
    public bool IsAllClear { get => wrapper.saveDatas.All(d=>d.status>=StageStatus.CLEAR); }

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
