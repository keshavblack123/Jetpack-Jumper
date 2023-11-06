using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariable", menuName = "ScriptableObjects/FloatVariable", order = 2)]
public class FloatVariable : Variable<float>
{
    public float currentValue;
    public override void SetValue(float value)
    {
        _value = value;
    }
    public void ApplyChange(float amount)
    {
        this.Value += amount;
    }
    override public string ToString()
    {
        return Value.ToString();
    }
}