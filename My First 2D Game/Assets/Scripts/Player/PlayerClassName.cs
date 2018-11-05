using UnityEngine;
using UnityEngine.UI;

public class PlayerClassName : MonoBehaviour
{
    public static string PlayerClass { get; set; }
    [SerializeField] Text ClassDisplay;
    public static string PlayerName { get; set; }
    [SerializeField] Text NameDisplay;

    void Start()
    {
        ClassDisplay.text = PlayerClass;
        NameDisplay.text = PlayerName;
    }
}
