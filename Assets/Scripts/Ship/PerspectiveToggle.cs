using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PerspectiveToggle : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private PlayableDirector _cameraToggleFade;

    #endregion

    #region private Variables

    private CameraManager _cameraManager;

    #endregion

    #region Unity Loops

    private void Start()
    {
        _cameraManager = FindObjectOfType<CameraManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Ship>() == null) { return; }

        StartCoroutine(OthoPerspectiveToggle(true));
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Ship>() == null) { return; }

        StartCoroutine(OthoPerspectiveToggle(false));
    }

    #endregion

    IEnumerator OthoPerspectiveToggle(bool boolean)
    {
        _cameraToggleFade.Play();
        yield return new WaitForSeconds(0.5f);
        _cameraManager.CullingMaskToggle(boolean);
    }
}