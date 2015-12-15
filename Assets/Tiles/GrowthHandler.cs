using UnityEngine;
using System.Collections;

public class GrowthHandler : MonoBehaviour
{
    int _health;
    public int Health
    {
        get
        {
            return _health;
        }

        set
        {
            if(_health != value)
            {
                _health = Mathf.Min(value, MaxHealth);
            }
        }
    }

    public int MaxHealth { get; set; }

    public int Level
    {
        get
        {
            return Mathf.Clamp(Health / 3, 0, 4);
        }

        set
        {
            Health = Mathf.Clamp((3 * value) + 1, 0, MaxHealth);
        }
    }


}
