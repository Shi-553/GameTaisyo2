using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideObjectUI : MonoBehaviour {
    [SerializeField]
    GameObject framePre;

    int currentIndex = -1;

    List<GameObject> objects = new List<GameObject>();

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    GameObject obj = null;
    public void SetItem(Sprite sprite) {
        if (obj != null) {
            Destroy(obj);
        }
        obj = Instantiate(framePre);
        obj.transform.SetParent(transform);
        obj.transform.position = transform.position;
        obj.GetComponent<Image>().sprite = sprite;
    }
    public void AddItem(Sprite sprite) {
        var obj = Instantiate(framePre);
        obj.transform.SetParent(transform);
        obj.GetComponent<Image>().sprite = sprite;
        objects.Add(obj);

        if (currentIndex < 0) {
            currentIndex = 0;
        }
    }
    public void RemoveItem() {
        if (currentIndex < 0) {
            return;
        }
        Destroy(objects[currentIndex]);
        objects.RemoveAt(currentIndex);

        currentIndex--;
    }
    public void NextItem() {
        if (currentIndex < 0) {
            currentIndex = 0;
        }
    }
    public void PrevItem() {

    }
}
