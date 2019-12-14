using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CutSene : MonoBehaviour
{
    [SerializeField]
    GameObject cutSceneCamera = null;
    GameObject endtext = null;
    [SerializeField]
    GameObject sceneloader=null;
    string m = "";
    private void Start()
    {
        endtext = GameObject.Find("EndMessage");
       
    }
    
    public void reciveMessage(string message)
    {
        m = message;
        StartCoroutine(ActiveCo());
    }
    IEnumerator ActiveCo()
    {
        yield return new WaitForSeconds(0.7f);
        cutSceneCamera.SetActive(true);
        endtext.GetComponent<Text>().text = m;
        endtext.GetComponent<Animator>().SetTrigger("go");
        yield return new WaitForSeconds(5f);
        sceneloader.SetActive(true);

    }
   
}
