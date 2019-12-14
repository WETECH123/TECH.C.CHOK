using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonNavigator : MonoBehaviour
{
    Button[] allButtons;
    public int buttonIndex = 0;
    [SerializeField]
    Transform _cursor=null;
    [SerializeField]
    Transform[] positions=null;
    public float r_input = 0;

    bool stop = false;
    bool right_input()
    {
        r_input = Input.GetAxis(StaticStrings.Right);
        r_input = r_input > 0 ? 1 : r_input;
        if(r_input==0)
        {
            stop = false;
        }
        return r_input >0;
       
    }

    bool left_input()
    {
        r_input = Input.GetAxis(StaticStrings.Right);
        r_input = r_input < 0 ? -1 : r_input;
        if (r_input == 0)
        {
            stop = false;
        }
        return r_input < 0;

    }

    private void Start()
    {
        allButtons = GetComponentsInChildren<Button>();
        allButtons[buttonIndex].Select();
        _cursor.transform.position = positions[0].transform.position;
    }
    private void Update()
    {
      
          
         if (right_input())
         {
            if (!stop)
            {
                stop = true;
                buttonIndex++;
                buttonIndex %= allButtons.Length;
                _cursor.transform.position = positions[buttonIndex].transform.position;
                allButtons[buttonIndex].Select();
            }
                
          }
        if (left_input())
        {
            if (!stop)
            {
                stop = true;
                buttonIndex--;
                if(buttonIndex < 0)
                {
                    buttonIndex = 0;
                }
                _cursor.transform.position = positions[buttonIndex].transform.position;
                allButtons[buttonIndex].Select();
            }

        }

        buttonFuntion();
    }


    public virtual void buttonFuntion()
    {
        if (Input.GetButtonDown(StaticStrings.X_key))
        {
            string n = allButtons[buttonIndex].GetComponent<SceneLoader>().scenename;
            allButtons[buttonIndex].GetComponent<SceneLoader>().Load(n);
        }
    }
}
