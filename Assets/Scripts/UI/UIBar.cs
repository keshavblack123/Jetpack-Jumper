using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour
{
    public GameConstants gameConstant;
    public FloatVariable jumpForce;
    public FloatVariable fuel;

    public Color color1;
    public Color color2;
    public Color color3;
    public Color color4;
    public Color color5;

    [Header("0 = jump, 1 = fuel")]
    public int type = 0;
    public float fillSpeed;
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
            fillAmount = Mathf.Lerp(
                fillAmount,
                fuel.Value / gameConstant.maxFuel,
                Time.deltaTime * fillSpeed
            );
        }

        if (fillAmount <= 0.2)
        {
            image.color = color1;
        }
        else if (fillAmount <= 0.4)
        {
            image.color = color2;
        }
        else if (fillAmount <= 0.6)
        {
            image.color = color3;
        }
        else if (fillAmount <= 0.8)
        {
            image.color = color4;
        }
        else if (fillAmount <= 1)
        {
            image.color = color5;
        }
        image.fillAmount = fillAmount;
    }
}
