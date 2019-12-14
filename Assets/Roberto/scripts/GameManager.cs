using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Header("UC Game Manager")]
    public string sceneToLoad = "";
    public Player PlayerPrefab;

    [HideInInspector]
    public Player LocalPlayer;

    private void Awake()
    {
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(sceneToLoad);
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
    }

   

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
    }
}
