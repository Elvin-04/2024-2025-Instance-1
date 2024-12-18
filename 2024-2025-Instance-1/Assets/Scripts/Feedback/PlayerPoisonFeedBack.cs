using Creators;
using Grid;
using JetBrains.Annotations;
using Player;
using System.Collections;
using TMPro;
using Traps;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerPoisonFeedBack : MonoBehaviour
{
    private void Start()
    {
        EventManager.instance.onClockUpdated?.AddListener(OnClockUpdate);
        EventManager.instance.onDeath?.AddListener(OnDeath);
        Assert.IsNotNull(_textPoison, "transform text poison is null in PlayerPoisonFeedBack");
        _originalColor = _spritePlayer.color;
        OnDeath();
    }

    private void Update()
    {
        if (!_poisonTrap) return;
        TextFeedBackPoison();
        LerpToLightColorThenOscillate();
    }

    #region Sprite FeedBack

    private void LerpToLightColorThenOscillate()
    {
        if (!_spritePlayer) return;

        if (!_hasReachedLightColor)
        {
            // Transition initiale de la couleur d'origine vers lightColor
            _lerpColorTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_lerpColorTimer / _durationLerpColor);
            _spritePlayer.color = Color.Lerp(_originalColor, _darkColor, t);

            // Réinitialise le timer pour l'oscillation
            if (t >= 1f)
            {
                _hasReachedLightColor = true;
                _lerpColorTimer = 0f;
            }
        }
        else
        {
            // Oscillation entre lightColor et darkColor
            _lerpColorTimer += Time.deltaTime;
            float t = Mathf.PingPong(_lerpColorTimer / _durationLerpColor, 1f);
            _spritePlayer.color = Color.Lerp(_darkColor, _lightColor, t);
        }
    }

    #endregion

    #region Action / Event

    public void OnClockUpdate()
    {
        if (!_isPoisoned) return;
        if (!gameObject.activeSelf) return;
        UpdatePoisonTextAfterDelay();
    }

    private void ActionIsPoisoned()
    {
        _isPoisoned = true;
        UpdatePoisonTextAfterDelay();
        EventManager.instance.onPoisonedPlayer?.RemoveListener(ActionIsPoisoned);
    }

    private void OnDeath(bool deathEffect = false)
    {
        // Text 
        _poisonTrap = null;
        _isPoisoned = false;
        _textPoison.text = string.Empty;
        EventManager.instance.onPoisonedPlayer?.AddListener(ActionIsPoisoned);

        // Sprite
        _lerpColorTimer = 0f;
        _spritePlayer.color = _originalColor;
    }

    #endregion

    #region Text FeedBack

    private void TextFeedBackPoison()
    {
        _timerAnimation += Time.deltaTime;
        VerticalTextFeedBackPoison();
    }

    private void VerticalTextFeedBackPoison()
    {
        // Oscillation verticale
        Vector3 originalPosition = _textPoison.rectTransform.localPosition;
        float offset = Mathf.Sin(_timerAnimation * _frequency) * _amplitude;

        _textPoison.rectTransform.localPosition = new Vector3(
            originalPosition.x,
            offset,
            originalPosition.z
        );
    }

    private void UpdatePoisonTextAfterDelay()
    {
        if (_poisonTrap)
        {
            SetPoisonText();
        }
        else
        {
            _poisonTrap = GetPoisonTrap();
            if (!_poisonTrap) return;

            _poisonTrapCreator = _poisonTrap.creator;
            SetPoisonText();
        }
    }

    private PoisonTrap GetPoisonTrap()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        GridManager gridManager = playerController.GetGridManager();
        Vector2Int nextIndex = gridManager.GetNextIndex(transform.position, playerController.MoveDirection());

        foreach (CellObjectBase trap in gridManager.GetObjectsOnCell(nextIndex))
            if (trap is PoisonTrap poisonTrap)
                return poisonTrap;

        return null;
    }

    private void SetPoisonText()
    {
        _textPoison.text = _poisonTrapCreator.GetCurrentTick().ToString();
        _textPoison.color = _poisonTextColor;
    }

    #endregion

    #region Parameters Sprite FeedBack Poison

    [Header("Sprite FeedBack Poison Parameters")]
    [SerializeField] private SpriteRenderer _spritePlayer;

    [SerializeField] private Color _darkColor;
    [SerializeField] private Color _lightColor;
    [SerializeField] private float _durationLerpColor = 0.5f;

    private Color _originalColor;
    private float _lerpColorTimer;
    private bool _hasReachedLightColor;

    #endregion

    #region Parameters Text FeedBack Poison

    [Header("Text FeedBack Poison Parameters")]
    [SerializeField] private TMP_Text _textPoison;

    [SerializeField] private Color _poisonTextColor;
    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _amplitude = 1f;

    private bool _isPoisoned;
    [CanBeNull] private PoisonTrap _poisonTrap;
    [CanBeNull] private PoisonTrapCreator _poisonTrapCreator;
    private float _timerAnimation;

    #endregion
}