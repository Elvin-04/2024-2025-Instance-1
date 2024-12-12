using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class StarAnimationManager : MonoBehaviour
    {
        private static readonly int IsOn = Animator.StringToHash("IsOn");
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        [SerializeField] private float _waitStarTime;
        [SerializeField] private Animator _star1;
        [SerializeField] private Animator _star2;
        [SerializeField] private Animator _star3;
        private float _currentScore = 3;
        private Animator _panelAnimator;

        private void Awake()
        {
            _panelAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            Assert.IsNotNull(_star1, "star1 is null in ScoreUI");
            Assert.IsNotNull(_star2, "star2 is null in ScoreUI");
            Assert.IsNotNull(_star3, "star3 is null in ScoreUI");
            EventManager.instance.onScoreUpdated?.AddListener(OnScoreUpdated);
        }

        private void OnEnable()
        {
            _star1.SetTrigger(IsIdle);
            _star2.SetTrigger(IsIdle);
            _star3.SetTrigger(IsIdle);
            
            StartCoroutine(StartStarAnim(_currentScore));
        }

        private void OnScoreUpdated(float score)
        {
            _currentScore = score;
        }

        private IEnumerator StartStarAnim(float score)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(_panelAnimator.GetCurrentAnimatorStateInfo(0).length + _waitStarTime);
            _star1.SetBool(IsOn, score >= 1);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(_star1.GetCurrentAnimatorClipInfo(0)[0].clip.length + _waitStarTime);
            _star2.SetBool(IsOn, score >= 2);
            yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(_star2.GetCurrentAnimatorClipInfo(0)[0].clip.length + _waitStarTime);
            _star3.SetBool(IsOn, score >= 3);
        }
    }
}