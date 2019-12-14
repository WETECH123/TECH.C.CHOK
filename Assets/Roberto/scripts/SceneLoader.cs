using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public bool active_X=false;
    public string scenename;
    
    private void Update()
    {
        if (active_X)
        {
            if (Input.GetButtonDown(StaticStrings.X_key))
            {
                Load(scenename);
            }
        }
    }
    public void Load(string scenename)
    {
        Soundmanager s = FindObjectOfType<Soundmanager>();
        if (s != null)
        {
            s.PlaySeByName(StaticStrings.EnterSoundEffect);
        }
        GameObject fadescreen = GameObject.Find("FadeScreen");
            if (fadescreen != null)
            {
                fadescreen.GetComponent<Animation>().Play("fade");
                  
            }
            Invoke("loadGAME", 3);
    }

    public void loadGAME()
    {
       SceneManager.LoadScene(scenename);
    }

}
