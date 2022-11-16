using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssetsManager : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private GameObject _damagePopup;

    #endregion

    #region Private Variables

    private static GameAssetsManager _instance;

    #endregion

    #region Public Properties

    public GameObject DamagePopup => _damagePopup;

    public static GameAssetsManager Instance { get { return _instance; } }

    #endregion

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}