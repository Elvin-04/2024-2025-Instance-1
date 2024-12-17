using Creators;
using JetBrains.Annotations;
using Player;
using System.Collections;
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

    private bool _isPoisoned;
    [CanBeNull] private PoisonTrap _poisonTrap;
    [CanBeNull] private PoisonTrapCreator _poisonTrapCreator;
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
        if (!_isPoisoned) return;
        if (!gameObject.activeSelf) return;
        StartCoroutine(UpdatePoisonTextAfterDelay());
    }

    private IEnumerator UpdatePoisonTextAfterDelay()
    {
        print("t3st");
        yield return null;

        if (_poisonTrap)
        {
            SetPoisonText();
        }
        else
        {
            _poisonTrap = GetPoisonTrap();
            if (!_poisonTrap) yield break;

            _poisonTrapCreator = _poisonTrap.creator;
            SetPoisonText();
        }

        if (_poisonTrap)
            Debug.Log("_poisonTrap.creator.GetPoisoned() : " + _poisonTrap.creator.GetPoisoned());
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
        if (_poisonTrapCreator.GetCurrentTick().ToString() == string.Empty)
            return;
        _textPoison.text = _poisonTrapCreator.GetCurrentTick().ToString();
        _textPoison.color = _poisonTextColor;
    }

    private void ActionIsPoisoned()
    {
        _isPoisoned = true;
        StartCoroutine(UpdatePoisonTextAfterDelay());
        EventManager.instance.onPoisonedPlayer?.RemoveListener(ActionIsPoisoned);
    }

    private void OnDeath(bool deathEffect = false)
    {
        _poisonTrap = null;
        _isPoisoned = false;
        _textPoison.text = string.Empty;
        EventManager.instance.onPoisonedPlayer?.AddListener(ActionIsPoisoned);
    }
}