using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceUIController : MonoBehaviour
{
    public FloatVariable fuel;
    public FloatVariable jumpForce;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI jumpText;
    // Update is called once per frame
    void Update()
    {
        fuelText.text = "Fuel: " + fuel.ToString();
        jumpText.text = "Jump: " + jumpForce.ToString();
    }
}
