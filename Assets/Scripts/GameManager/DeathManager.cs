using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class DeathManager : MonoBehaviour
{
    #region Editor Fields

    [SerializeField] private Volume _postProcessing;

    #endregion

    #region Private Variables

    private ShipHealth _shipHealth;
    private float _timeSinceDeath;

    #endregion

    private void Start()
    {
        _shipHealth = Ship.Instance.GetComponent<ShipHealth>();
    }

    private void Update()
    {
        if (!_shipHealth.IsDead()) { _timeSinceDeath = 0f; return; }

        _timeSinceDeath += Time.deltaTime;

        if(_timeSinceDeath < Ship.Instance.TimeTillDeath) { return; }

        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        KillAllPlayers();

        yield return new WaitForSeconds(2f);

        ShowDeathScreen();

        yield return new WaitForSeconds((float)DeathPanelUI.Instance.DeathPanelTimeLine.duration);

        ReloadScene();
    }

    private void ShowDeathScreen()
    {
        DeathPanelUI.Instance.PlayDeathTimeline();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(2);
    }

    private void KillAllPlayers()
    {
        _timeSinceDeath = 0f;

        PlayerHealth[] allPlayers = FindObjectsOfType<PlayerHealth>();

        foreach(PlayerHealth player in allPlayers)
        {
            player.Hurt(DamageType.Base);
        }
    }

    private void AddEyeClosingFX()
    {
        VolumeInterface.Instance.ChangeVignetteByPercentage(_timeSinceDeath / Ship.Instance.TimeTillDeath);
    }
}
