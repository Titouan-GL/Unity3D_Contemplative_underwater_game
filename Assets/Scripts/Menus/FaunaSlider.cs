using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FaunaSlider : MonoBehaviour
{
    public static float faunaDensity = 1;
    public TMP_Text faunaDensityText;
    public Slider faunaSlider;

    public static float Sensitivity = 20;
    public TMP_Text bPrecText;
    public Slider sensitivitySlider;

    // Update is called once per frame
    void Start(){
        faunaSlider.value = faunaDensity;
        sensitivitySlider.value =  Sensitivity;
    }
    void Update()
    {
        faunaDensity = faunaSlider.value;
        faunaDensityText.text = "Fauna Density : " + (faunaDensity).ToString("0.00");

        Sensitivity = sensitivitySlider.value;
        bPrecText.text = "Sensitivity : " + (Sensitivity).ToString();

    }
}
