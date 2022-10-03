using UnityEngine;

//This is an empty class used to locate the ship in the Hierarchy
public class Ship : MonoBehaviour
{
    #region Private Variables

    private static Ship _instance;

    #endregion

    #region Public Properties

    public static Ship Instance { get { return _instance; } }

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
