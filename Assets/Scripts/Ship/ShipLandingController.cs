using UnityEngine;

public class ShipLandingController : MonoBehaviour
{
    #region Editor Fields

    [Header("Components")]
    [SerializeField] private Booster _booster;

    [Header("Landing Gear")]
    [SerializeField] private Transform _landingGearTransform;
    [SerializeField] private float _landingGearStoredYPos;
    [SerializeField] private float _landingGearDeployedYPos;

    #endregion

    #region Getters && Setters

    public bool IsWantedDeployed { get { return _isWantedDeployed; } set { _isWantedDeployed = value; } }

    #endregion

    #region Private Variables

    private bool _isWantedDeployed = false;
    private bool _isLandingGearDeployed = false;

    #endregion

    #region Unity Loops

    private void Start()
    {
        PlayerInputHandler.OnSpecialAction += HandleSpecialAction;
    }

    private void FixedUpdate()
    {
        if (_isWantedDeployed == _isLandingGearDeployed) { return; }

        float wantedYPosition = _isLandingGearDeployed == true ? _landingGearStoredYPos : _landingGearDeployedYPos;

        _landingGearTransform.localPosition = new
            Vector3(_landingGearTransform.localPosition.x,
            Mathf.Lerp(_landingGearTransform.localPosition.y, wantedYPosition, Time.deltaTime), _landingGearTransform.localPosition.z);

        if (Mathf.RoundToInt(_landingGearTransform.localPosition.y) == Mathf.RoundToInt(wantedYPosition)) { _isLandingGearDeployed = _isWantedDeployed; }
    }

    private void OnDestroy()
    {
        PlayerInputHandler.OnSpecialAction -= HandleSpecialAction;
    }

    #endregion

    private void HandleSpecialAction(PlayerInputHandler player, bool isPressed)
    {
        if(_isWantedDeployed != _isLandingGearDeployed) { return; }

        PlayerInteractionController playerInteractionController = _booster.CurrentPlayer as PlayerInteractionController;

        //If player that pressed action button is not on booster, return
        if (player != playerInteractionController?.PlayerInput) { return; }

        if (!isPressed) { return; }

        _isWantedDeployed = !_isWantedDeployed;
    }
}
