using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ChargeManager : MonoBehaviour
{
    public GameObject player;
    public Image chargeBar;

    public Color thirdColor;
    public Color twothirdColor;
    public Color fullColor;

    public GameConstants gameConstant;
    public FloatVariable jumpForce;

    public Vector3 offset;
    public Vector2 screenPosition;

    // Start is called before the first frame update
    void Start()
    {
        chargeBar = GameObject.Find("ChargeBarImg").GetComponent<Image>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Follow Player Pos
        screenPosition = Camera.main.WorldToScreenPoint(player.transform.position + offset);
        GetComponent<RectTransform>().anchoredPosition = screenPosition;

        // Animate The Charge Bar
        float fillValue =
            (jumpForce.Value - gameConstant.startingJumpForce)
            / (30f - gameConstant.startingJumpForce);

        chargeBar.fillAmount = fillValue;

        chargeBar.color =
            (fillValue <= 0.4f)
                ? thirdColor
                : (fillValue <= 0.7f)
                    ? twothirdColor
                    : fullColor;

        // Shake The Charge Bar When Full
        if (fillValue >= 1) { }
        else { }
    }
}
