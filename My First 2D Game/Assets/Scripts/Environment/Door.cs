using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform ConnectedDoor;
    public Transform GetPosition //ConnectedDoor's getter
    {
        get { return ConnectedDoor; }
    }
}
