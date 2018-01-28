using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodyScreenUI : MonoBehaviour {
    [SerializeField]
    private Image bloodImage;

    public void OnBloodyScreen() {
        StartCoroutine(Fade(Color.clear, new Color(1, 1, 1, .4f), .5f));
    }

    IEnumerator Fade(Color from, Color to, float time) {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1) {
            percent += Time.deltaTime * speed;
            bloodImage.color = Color.Lerp(from, to, percent);
            yield return null;
        }
    }
}
