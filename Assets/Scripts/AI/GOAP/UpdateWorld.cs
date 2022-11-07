using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour
{
    public Text States;

    private void LateUpdate()
    {
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
        States.text = "";
        foreach (KeyValuePair<string, int> state in worldStates)
        {
            States.text += state.Key + ", " + state.Value + "\n";
        }
    }
}
