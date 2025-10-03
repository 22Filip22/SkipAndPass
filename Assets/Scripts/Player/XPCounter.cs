using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Text;
using System.Collections;

public class XPCounter : MonoBehaviour
{
    public static XPCounter Instance { get; private set; }

    [Header("Level Settings")]
    [SerializeField] private int maxLevel = 30;

    [Header("XP Display")]
    [SerializeField] private TMP_Text xpBarText;      // TextMeshPro field in Inspector
    [SerializeField] private string pipeSymbol = "|"; // Symbol for XP unit
    private const int MaxPipeDisplay = 205;           // Max number of symbols

    [SerializeField]
    TextMeshProUGUI levelText;

    public int CurrentLevel { get; private set; } = 1;
    public int CurrentLevelXP { get; private set; }
    public int TotalXP { get; private set; }

    [Header("Events")]
    [SerializeField] private UnityEvent onLevelUp = new UnityEvent();
    public UnityEvent OnLevelUp => onLevelUp;

    public int XPForNextLevel => GetXPRequirementForLevel(CurrentLevel);

    private Coroutine xpAnimationCoroutine;
    private int currentSymbols = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        AnimateXPAdd();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0 || CurrentLevel >= maxLevel) return;

        TotalXP += amount;
        CurrentLevelXP += amount;

        if (xpAnimationCoroutine != null)
            StopCoroutine(xpAnimationCoroutine);

        xpAnimationCoroutine = StartCoroutine(AnimateXPAdd());
    }

    private IEnumerator AnimateXPAdd()
    {
        while (CurrentLevelXP > 0 && CurrentLevel < maxLevel)
        {
            int needed = GetXPRequirementForLevel(CurrentLevel);
            int targetSymbols = Mathf.RoundToInt((float)CurrentLevelXP / needed * MaxPipeDisplay);
            targetSymbols = Mathf.Clamp(targetSymbols, 0, MaxPipeDisplay);

            while (currentSymbols < targetSymbols)
            {
                currentSymbols++;
                UpdateXPBar();
                yield return new WaitForSeconds(0.01f);
            }

            // When bar is full, level up
            if (currentSymbols >= MaxPipeDisplay)
            {
                CurrentLevelXP -= needed;
                CurrentLevel++;
                levelText.text = $"Level: {CurrentLevel}";
                onLevelUp.Invoke();
                Debug.Log("Level Up!");

                // Reset bar for next level if not max level
                currentSymbols = 0;
                UpdateXPBar();
                yield return null;
            }
            else
            {
                // No more XP to animate
                break;
            }
        }

        // Clamp values if max level reached
        if (CurrentLevel >= maxLevel)
        {
            CurrentLevel = maxLevel;
            CurrentLevelXP = 0;
            currentSymbols = MaxPipeDisplay;
            UpdateXPBar();
        }
    }

    private void UpdateXPBar()
    {
        if (xpBarText == null) return;

        var sb = new StringBuilder(currentSymbols);
        for (int i = 0; i < currentSymbols; i++)
            sb.Append(pipeSymbol);

        xpBarText.text = sb.ToString();
    }

    public void ResetAll()
    {
        CurrentLevel = 1;
        CurrentLevelXP = 0;
        TotalXP = 0;
        currentSymbols = 0;
        UpdateXPBar();
        levelText.text = $"Level: {CurrentLevel}";
    }

    private int GetXPRequirementForLevel(int level)
    {
        if (level >= maxLevel) return 0;

        if (level >= 1 && level < 5) return 100;
        if (level >= 5 && level < 10) return 200;
        if (level >= 10 && level < 20) return 400;
        if (level >= 20 && level < 30) return 800;

        return 1000;
    }

    public int GetTotalXPRequiredForLevel(int targetLevel)
    {
        if (targetLevel <= 1) return 0;
        if (targetLevel > maxLevel) targetLevel = maxLevel;

        int sum = 0;
        for (int lvl = 1; lvl < targetLevel; lvl++)
            sum += GetXPRequirementForLevel(lvl);
        return sum;
    }
}
