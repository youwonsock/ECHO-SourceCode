using System.ComponentModel;
using UnityEngine;

public class PlayerPresenter : MonoBehaviour
{
    // model
    [SerializeField] private MovePlayer movePlayer;
    [SerializeField] private QTESystem qteSystem;
    [SerializeField] private FollowCamera followCamera;

    // view
    public IProgressUIView progressUIView = null;



    public float MouseSensitivity
    {
        set 
        {
            if(movePlayer != null)
                movePlayer.MouseSensitivity = value; 
        }
    }

    public float CameraFOV
    {
        set 
        {
            if(followCamera != null)
                followCamera.CameraFOV = value; 
        }
    }


    public void SetPlayerModel()
    {
        // find Player and set medel data and event
        GameObject.FindGameObjectWithTag("Player").TryGetComponent<MovePlayer>(out movePlayer);
        qteSystem = GameObject.FindGameObjectWithTag("CutScenePlayer").transform.GetChild(0).GetComponent<QTESystem>();
        GameObject.FindGameObjectWithTag("MainCamera").TryGetComponent<FollowCamera>(out followCamera);

        movePlayer.PropertyChanged += UpdateUI;
        qteSystem.PropertyChanged += UpdateUI;
    }

    public void UpdateUI(object sender, PropertyChangedEventArgs e)
    {
        if(progressUIView != null)
        {
            if (e.PropertyName == "CurrentStamina")
            {
                progressUIView.UpdateUI(movePlayer.CurrentStamina);
            }
            else if (e.PropertyName == "Progressbar")
            { 
                progressUIView.UpdateUI(qteSystem.Progressbar * 0.01f);
            }
        }
    }
}
