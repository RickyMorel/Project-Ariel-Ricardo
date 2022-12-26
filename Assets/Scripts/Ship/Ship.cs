using UnityEngine;

//This is an empty class used to locate the ship in the Hierarchy
public class Ship : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private float _topSpeed = 60f;

    #endregion

    #region Private Variables

    private static Ship _instance;

    #endregion

    #region Public Properties

    public static Ship Instance { get { return _instance; } }
    public float TopSpeed => _topSpeed;

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
