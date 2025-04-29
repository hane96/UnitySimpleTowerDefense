using UnityEngine;
using TMPro;

public class castleUI: MonoBehaviour
{
    [SerializeField] private castle castleScript;
    [SerializeField] private TextMeshProUGUI healthtext;

    void Update()
    {
        if(castleScript.GetHealth()<=0) 
        {
            healthtext.text = "Game over";
        }
        else healthtext.text = "HP: "+castleScript.GetHealth().ToString();
        
    }
}
