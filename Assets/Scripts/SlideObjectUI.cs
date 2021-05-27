using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideObjectUI : MonoBehaviour {
    [SerializeField]
    GameObject framePre;
    [SerializeField]
    GameObject selectFramePre;
    [SerializeField]
    Transform selectX;

    List<Sprite> currentSprites ;
    List<GameObject> objects = new List<GameObject>();

    [SerializeField]
    Vector3 nextPosDiff;

    [SerializeField]
    AudioClip selectSe;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void SetItem(List<Sprite> sprites, int currentIndex) {
        currentSprites = sprites;
        foreach (var obj in objects) {

            if (obj != null) {
                Destroy(obj);
            }
        }
        objects.Clear();

        for (int i = 0; i < 3; i++) {

            var obj = (i == currentIndex) ? Instantiate(selectFramePre) : Instantiate(framePre);

            obj.transform.SetParent(transform);
            obj.transform.position = transform.position;
            obj.transform.position -= nextPosDiff * (i - 1f);


            if (sprites.Count > i) {
                obj.transform.GetChild(0).GetComponent<Image>().sprite = sprites[i];
            }
            else {
                obj.transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
            objects.Add(obj);

        }
        if (objects.Count > currentIndex) {
            selectX.position = objects[currentIndex].transform.position - new Vector3(50, 50);
        }
    }
    public void SeIndex(int currentIndex) {
        SetItem(currentSprites,currentIndex);
        AudioManager.Instance.Play(selectSe);
    }
}
