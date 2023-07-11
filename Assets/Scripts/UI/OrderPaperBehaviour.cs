using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPaperBehaviour : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI text;

    public void SetTableNumber(int number)
    {
        text.text = number.ToString();
    }
}
