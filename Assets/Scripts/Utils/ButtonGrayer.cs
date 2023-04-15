using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonGrayer : MonoBehaviour
{
    Button button;
    List<TextMeshProUGUI> text = new List<TextMeshProUGUI>();
    List<Image> image = new List<Image>();

    void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentsInChildren<TextMeshProUGUI>().ToList();
        image = GetComponentsInChildren<Image>().ToList();

        if (button.interactable) {
            foreach (var item in text) {
                item.CrossFadeAlpha(1, 0, false);
            }
            foreach (var item in image) {
                item.CrossFadeAlpha(1, 0, false);
            }
        } else {
            foreach (var item in text) {
                item.CrossFadeAlpha(0.4f, 0, false);
            }
            foreach (var item in image) {
                item.CrossFadeAlpha(0.4f, 0, false);
            }
        }
    }

    void Update()
    {
        if (button.interactable) {
            foreach (var item in text) {
                item.CrossFadeAlpha(1, 0, false);
            }
            foreach (var item in image) {
                item.CrossFadeAlpha(1, 0, false);
            }
        } else {
            foreach (var item in text) {
                item.CrossFadeAlpha(0.4f, 0, false);
            }
            foreach (var item in image) {
                item.CrossFadeAlpha(0.4f, 0, false);
            }
        }
    }
}
