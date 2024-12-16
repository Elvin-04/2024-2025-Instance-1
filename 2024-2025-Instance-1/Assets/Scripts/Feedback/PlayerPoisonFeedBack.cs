using System.Collections;
using JetBrains.Annotations;
using Player;
using TMPro;
using Traps;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class PlayerPoisonFeedBack : MonoBehaviour
{
    [FormerlySerializedAs("_transformTextPoison")]
    [Header("FeedBack Poison Parameters")]
    [SerializeField] private TMP_Text _textPoison;

    [SerializeField] private Color _poisonTextColor;
    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _amplitude = 1f;

    [CanBeNull] private PoisonTrap _poisonTrap;
    private float _timerAnimation;


    private void Start()
    {
        EventManager.instance.onClockUpdated?.AddListener(OnClockUpdate);
        EventManager.instance.onDeath?.AddListener(OnDeath);

        Assert.IsNotNull(_textPoison, "transform text poison is null in PlayerPoisonFeedBack");
        OnDeath();
    }

    private void Update()
    {
        if (!_poisonTrap) return;
        FeedBackPoison();
    }

    private void FeedBackPoison()
    {
        _timerAnimation += Time.deltaTime;
        VerticalFeedBackPoison();
    }

    private void VerticalFeedBackPoison()
    {
        // Oscillation verticale
        var originalPosition = _textPoison.rectTransform.localPosition;
        var offset = Mathf.Sin(_timerAnimation * _frequency) * _amplitude;

        _textPoison.rectTransform.localPosition = new Vector3(
            originalPosition.x,
            offset,
            originalPosition.z
        );
    }

    public void OnClockUpdate()
    {
        StartCoroutine(UpdatePoisonTextAfterDelay());
    }

    private IEnumerator UpdatePoisonTextAfterDelay()
    {
        yield return null;

        if (_poisonTrap)
        {
            SetPoisonText();
        }
        else
        {
            _poisonTrap = GetPoisonTrap();
            if (_poisonTrap) SetPoisonText();
        }
    }

    private PoisonTrap GetPoisonTrap()
    {
        var playerController = GetComponent<PlayerController>();
        var gridManager = playerController.GetGridManager();
        var nextIndex = gridManager.GetNextIndex(transform.position, playerController.MoveDirection());

        foreach (var trap in gridManager.GetObjectsOnCell(nextIndex))
            if (trap is PoisonTrap poisonTrap)
                return poisonTrap;

        return null;
    }

    private void SetPoisonText()
    {
        _textPoison.text = _poisonTrap.currentLifeTime.ToString();
        _textPoison.color = _poisonTextColor;
    }

    private void OnDeath()
    {
        _poisonTrap = null;
        _textPoison.text = string.Empty;
    }
}