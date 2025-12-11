using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// Handles rebinding keys in the pause menu and persists them with PlayerPrefs.
/// Works with the InputManager key fields.
/// </summary>
public class RebindManager : MonoBehaviour
{
    [Header("References")]
    public InputManager inputManager; // reference to your InputManager

    [Header("UI References")]
    public TMP_Text upText;
    public TMP_Text downText;
    public TMP_Text leftText;
    public TMP_Text rightText;
    public TMP_Text runText;
    public TMP_Text jumpText;
    public TMP_Text dashText;
    public TMP_Text skillText;
    public TMP_Text zoomInText;
    public TMP_Text zoomOutText;

    private bool waitingForKey = false;
    private TMP_Text waitingText;

    void Start()
    {
        // Load saved keys
        LoadKeyBindings();
        UpdateAllUIText();
    }

    void Update()
    {
        if (waitingForKey)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    ApplyKey(key);
                    break;
                }
            }
        }
    }

    // -----------------------------
    // UI Button Calls
    // -----------------------------
    public void RebindUp() => StartListening(upText);
    public void RebindDown() => StartListening(downText);
    public void RebindLeft() => StartListening(leftText);
    public void RebindRight() => StartListening(rightText);
    public void RebindRun() => StartListening(runText);
    public void RebindJump() => StartListening(jumpText);
    public void RebindDash() => StartListening(dashText);
    public void RebindSkill() => StartListening(skillText);
    public void RebindZoomIn() => StartListening(zoomInText);
    public void RebindZoomOut() => StartListening(zoomOutText);

    private void StartListening(TMP_Text textField)
    {
        if (waitingForKey) return;

        waitingText = textField;
        waitingForKey = true;
        waitingText.text = "Press a key...";
    }

    private void ApplyKey(KeyCode newKey)
    {
        if (waitingText == null)
        {
            Debug.LogWarning("ApplyKey called but waitingText is null!");
            waitingForKey = false;
            return;
        }

        waitingForKey = false;

        // Assign key
        if (waitingText == upText) inputManager.upKey = newKey;
        else if (waitingText == downText) inputManager.downKey = newKey;
        else if (waitingText == leftText) inputManager.leftKey = newKey;
        else if (waitingText == rightText) inputManager.rightKey = newKey;
        else if (waitingText == runText) inputManager.runKey = newKey;
        else if (waitingText == jumpText) inputManager.jumpKey = newKey;
        else if (waitingText == dashText) inputManager.dashKey = newKey;
        else if (waitingText == skillText) inputManager.skillKey = newKey;
        else if (waitingText == zoomInText) inputManager.zoomInKey = newKey;
        else if (waitingText == zoomOutText) inputManager.zoomOutKey = newKey;

        // Save
        PlayerPrefs.SetString(waitingText.name, newKey.ToString());
        PlayerPrefs.Save();

        UpdateUIText(waitingText, newKey);

        waitingText = null;
    }

    private void UpdateUIText(TMP_Text textField, KeyCode key)
    {
        textField.text = key.ToString();
    }

    private void UpdateAllUIText()
    {
        upText.text = inputManager.upKey.ToString();
        downText.text = inputManager.downKey.ToString();
        leftText.text = inputManager.leftKey.ToString();
        rightText.text = inputManager.rightKey.ToString();
        runText.text = inputManager.runKey.ToString();
        jumpText.text = inputManager.jumpKey.ToString();
        dashText.text = inputManager.dashKey.ToString();
        skillText.text = inputManager.skillKey.ToString();
        zoomInText.text = inputManager.zoomInKey.ToString();
        zoomOutText.text = inputManager.zoomOutKey.ToString();
    }

    private void LoadKeyBindings()
    {
        // Only overwrite defaults if PlayerPrefs exist
        if (PlayerPrefs.HasKey(upText.name)) 
            inputManager.upKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(upText.name));
        if (PlayerPrefs.HasKey(downText.name)) 
            inputManager.downKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(downText.name));
        if (PlayerPrefs.HasKey(leftText.name)) 
            inputManager.leftKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(leftText.name));
        if (PlayerPrefs.HasKey(rightText.name)) 
            inputManager.rightKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(rightText.name));
        if (PlayerPrefs.HasKey(runText.name)) 
            inputManager.runKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(runText.name));
        if (PlayerPrefs.HasKey(jumpText.name)) 
            inputManager.jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(jumpText.name));
        if (PlayerPrefs.HasKey(dashText.name)) 
            inputManager.dashKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(dashText.name));
        if (PlayerPrefs.HasKey(skillText.name)) 
            inputManager.skillKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(skillText.name));
        if (PlayerPrefs.HasKey(zoomInText.name)) 
            inputManager.zoomInKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(zoomInText.name));
        if (PlayerPrefs.HasKey(zoomOutText.name))
            inputManager.zoomOutKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString(zoomOutText.name));
    }

    // Optional: Reset all keys to defaults
    public void ResetToDefaults()
    {
        inputManager.upKey = KeyCode.W;
        inputManager.downKey = KeyCode.S;
        inputManager.leftKey = KeyCode.A;
        inputManager.rightKey = KeyCode.D;
        inputManager.runKey = KeyCode.LeftShift;
        inputManager.jumpKey = KeyCode.Space;
        inputManager.dashKey = KeyCode.LeftControl;
        inputManager.skillKey = KeyCode.E;
        inputManager.zoomInKey = KeyCode.KeypadPlus;      // + key (same key as =)
        inputManager.zoomOutKey = KeyCode.KeypadMinus; 

        // Remove PlayerPrefs
        PlayerPrefs.DeleteKey(upText.name);
        PlayerPrefs.DeleteKey(downText.name);
        PlayerPrefs.DeleteKey(leftText.name);
        PlayerPrefs.DeleteKey(rightText.name);
        PlayerPrefs.DeleteKey(runText.name);
        PlayerPrefs.DeleteKey(jumpText.name);
        PlayerPrefs.DeleteKey(dashText.name);
        PlayerPrefs.DeleteKey(skillText.name);
        PlayerPrefs.DeleteKey(zoomInText.name);
        PlayerPrefs.DeleteKey(zoomOutText.name);

        UpdateAllUIText();
    }
}
