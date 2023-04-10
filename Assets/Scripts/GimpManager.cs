using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimpManager : MonoBehaviour
{
    [SerializeField]
    int currentGimp;
    [SerializeField]
    int maxGimp;

    [SerializeField]
    Slider gimpMeterSlider;
    [SerializeField]
    RectTransform gimpMeterContainer;

    bool updateGimpMeter;
    [SerializeField]
    float gimpContainerPadding;

    private void Start() {
        updateGimpMeter = true;
    }

    private void Update() {
        // We do this because there might be multiple changes to the gimp charge in one frame. It would be unnecessary to update the meter for each one since it can only be drawn once each frame anyway.
        if (updateGimpMeter) {
            gimpMeterSlider.maxValue = maxGimp;
            gimpMeterSlider.value = currentGimp;

            gimpMeterSlider.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * maxGimp);

            gimpMeterContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 10 * maxGimp + gimpContainerPadding);

            Debug.Log(currentGimp);
            updateGimpMeter = false;
        }
    }

    public bool SpendGimp(int value) {
        if (currentGimp >= value) {
            currentGimp -= value;
            updateGimpMeter = true;
            return true;
        }
        return false;
    }
}
