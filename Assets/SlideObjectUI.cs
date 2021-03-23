using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlideObjectUI : MonoBehaviour {
    [SerializeField]
    GameObject framePre;
    [SerializeField]
    GameObject selectFramePre;


    List<GameObject> objects = new List<GameObject>();

    [SerializeField]
    Vector3 nextPosDiff;
    [SerializeField]
    Vector3 nextScaleDiff;
    [SerializeField]
    Vector3 nextRotateDiff;

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }


    public void SetItem(List<Sprite> sprites, int currentIndex, int halfSiblingCount) {
        foreach (var obj in objects) {
            if (obj != null) {
                Destroy(obj);
            }
        }

        var index = currentIndex - halfSiblingCount;
        while (index < 0) {
            index += sprites.Count;
        }
        index %= sprites.Count;

        for (int i = 0; i < halfSiblingCount * 2 + 1; i++) {
            var sprite = sprites[index];

            var obj = (index  == currentIndex )? Instantiate(selectFramePre) : Instantiate(framePre);

            obj.transform.SetParent(transform);
            obj.transform.position = transform.position;
            var nextPos = nextPosDiff;
            if (i > sprites.Count / 2) {
                nextPos.y *= -1;
            }
            obj.transform.position -= nextPos * (i - sprites.Count / 2);

            var nextScale = nextScaleDiff;
            if (i > sprites.Count / 2) {
                nextScale *= -1;
            }
            obj.transform.localScale -= nextScale * (i - sprites.Count / 2);

            var nextrotate = nextRotateDiff;
            obj.transform.localEulerAngles -= nextrotate * (i - sprites.Count / 2);


            obj.transform.GetChild(0).GetComponent<Image>().sprite = sprite;

            objects.Add(obj);

            index = (index + 1 + sprites.Count) % sprites.Count;
        }
    }
}
