using UnityEngine;

[CreateAssetMenu(fileName = "GameConstants", menuName = "ScriptableObjects/GameConstants", order = 1)]
public class GameConstants : ScriptableObject
{
    [Header("Player Configs")]
    public float startingJumpForce = 5f;
    public float startingFuel = 50f;
    public float delayTime = 10;
    public float maxFuel = 50;
    public LayerMask jumpableGround;
    public float fuelIncrement = .1f;

}