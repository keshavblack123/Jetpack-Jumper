using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public GameConstants gameConstant;
    public FloatVariable jumpForce;
    public FloatVariable fuel;

    [Header("0 = jump, 1 = fuel")]
    public int type = 0;
    private Image image;
    private float fillAmount = 0;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (type == 0)
        {
            fillAmount =
                (jumpForce.Value - gameConstant.startingJumpForce)
                / (30f - gameConstant.startingJumpForce);
        }
        else
        {
            fillAmount = fuel.Value / gameConstant.maxFuel;
        }
        image.fillAmount = fillAmount;
    }
}
