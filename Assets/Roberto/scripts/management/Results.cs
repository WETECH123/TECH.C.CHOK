using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Results : MonoBehaviour
{
    [SerializeField]
    Text resultText=null;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey(StaticStrings.result))
        {
            resultText.text = PlayerPrefs.GetString(StaticStrings.result);
        }
        else
        {
            resultText.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
