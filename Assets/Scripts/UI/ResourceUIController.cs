using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUIController : MonoBehaviour
{
    public FloatVariable fuel;
    public FloatVariable jumpForce;
    public GameConstants gameConstants;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI jumpText;

    // Update is called once per frame
    void Update()
    {
        // fuelText.text = "Fuel: " + fuel.ToString();
        // jumpText.text = "Jump: " + jumpForce.ToString();
        fuelText.text = "Fuel: " + Mathf.Clamp(fuel.Value, 0f, gameConstants.maxFuel).ToString("F1");
        jumpText.text = "Jump: " + jumpForce.Value.ToString("F1");
    }

}
