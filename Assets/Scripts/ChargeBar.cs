using UnityEngine;

public class ChargeBar : MonoBehaviour
{
    private SpriteRenderer chargeBarSprite;

    public Color fullChargeColor = Color.green;
    public Color emptyChargeColor = Color.red;

    public Gradient colorGradient;

    public Gradient startColorGradient;
    public Gradient endColorGradient;

    public GameConstants gameConstant;
    public FloatVariable jumpForce;

    private float initialPosX;
    private float initialScaleX;

    private void Start()
    {
        initialPosX = transform.localPosition.x;
        initialScaleX = transform.localScale.x;
        chargeBarSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        // Logic to change color
        float fillAmount =
            (jumpForce.Value - gameConstant.startingJumpForce)
            / (30f - gameConstant.startingJumpForce)
            * initialScaleX;
        //chargeBarSprite.color = Color.Lerp(emptyChargeColor, fullChargeColor, fillAmount / initialScaleX); // between 2 colors
        //chargeBarSprite.color = colorGradient.Evaluate(fillAmount / initialScaleX); // only 1 gradient
        Color startColor = startColorGradient.Evaluate(fillAmount / initialScaleX);
        Color endColor = endColorGradient.Evaluate(fillAmount / initialScaleX);

        // Use Color.Lerp to blend between the start and end colors
        chargeBarSprite.color = Color.Lerp(startColor, endColor, fillAmount);

        // Sprite only expands to the right
        chargeBarSprite.transform.localScale = new Vector3(
            fillAmount,
            transform.localScale.y,
            transform.localScale.z
        );
        // Calc new position for the sprite so it "stays" in place
        float newX = -(initialScaleX - fillAmount) * 0.5f + initialPosX;
        chargeBarSprite.transform.localPosition = new Vector3(
            newX,
            transform.localPosition.y,
            transform.localPosition.z
        );
    }
}
