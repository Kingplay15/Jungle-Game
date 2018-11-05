using UnityEngine;
using UnityEngine.UI;

public class CreateCharacter : MonoBehaviour
{
    [SerializeField] Text EnteredName; 

    public void InitClass(string Class)
    {
        PlayerClassName.PlayerClass = Class;
    }

    public void InitName()
    {
        PlayerClassName.PlayerName = EnteredName.text;
    }
}
