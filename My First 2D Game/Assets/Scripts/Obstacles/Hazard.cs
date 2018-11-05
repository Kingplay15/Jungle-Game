using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] int Damage;
    public int GetDamage
    {
        get { return Damage; }
    }
}
