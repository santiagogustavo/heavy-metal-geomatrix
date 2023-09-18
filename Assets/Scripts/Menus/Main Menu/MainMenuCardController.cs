using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuCardController : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler {
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;

    [SerializeField]
    bool bypassDim;

    Animator animator;
    Image image;
    RectTransform rectTransform;
    Vector3 originalPosition;
    Vector3 hidePosition;
    Color originalColor;
    Color dimColor;

    bool isSelected;
    float startTime;

    void Awake() {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        originalColor = image.color;
        dimColor = new Color(originalColor.r - 0.5f, originalColor.g - 0.5f, originalColor.b - 0.5f);
        originalPosition = rectTransform.localPosition;
        hidePosition = new Vector3(originalPosition.x - 25f, originalPosition.y, originalPosition.z);
    }

    public void OnPointerEnter(PointerEventData pd) {
        MainMenuManager.instance.SelectCard(gameObject.name);
    }

    public void OnPointerClick(PointerEventData pd) {
        MainMenuManager.instance.SelectCurrent();
    }

    void Update() {
        if (MainMenuManager.instance.IsSelected()) {
            animator.Play("Card Close");
        }
        if (bypassDim) {
            return;
        }
        float lerp = (Time.time - startTime) * 20f;
        if (MainMenuManager.instance.IsCardSelected(gameObject.name)) {
            if (!isSelected) {
                startTime = Time.time;
                isSelected = true;
            }
            image.color = originalColor;
            rectTransform.localPosition = Vector3.Lerp(hidePosition, originalPosition, lerp);
        } else {
            if (isSelected) {
                startTime = Time.time;
                isSelected = false;
            }
            image.color = dimColor;
            rectTransform.localPosition = Vector3.Lerp(originalPosition, hidePosition, lerp);
        }
    }
}
