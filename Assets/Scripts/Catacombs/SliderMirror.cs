using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HorrorVR
{
    /// <summary>
    /// Mirrors the normalised value from the reference slider to all the other sliders
    /// </summary>
    public class SliderMirror : MonoBehaviour
    {
        [SerializeField]
        private Slider referenceSlider;

        [SerializeField]
        private Slider[] sliders;

        private void OnEnable()
        {
            referenceSlider.onValueChanged.AddListener(ChangeValue);
        }

        private void OnDisable()
        {
            referenceSlider.onValueChanged.RemoveListener(ChangeValue);
        }

        private void ChangeValue(float value)
        {
            foreach (var slider in sliders)
            {
                slider.normalizedValue = referenceSlider.normalizedValue;
            }
        }
    }
}
