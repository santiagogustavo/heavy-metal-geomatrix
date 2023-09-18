using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OptionsMenuVolumeControl : MonoBehaviour, IPointerClickHandler {
    [SerializeField] OptionsMenuVolumeOption option;

    public void OnPointerClick(PointerEventData pd) {
        OptionsMenuManager.instance.HandleVolumeEvent(option);
    }
}
