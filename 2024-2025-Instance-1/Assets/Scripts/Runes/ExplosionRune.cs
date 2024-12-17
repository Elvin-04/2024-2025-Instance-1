using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Runes
{
    public class ExplosionRune : Rune, IZone
    {
        [SerializeField] private GameObject _explosionPrefab;
        [SerializeField] private int _radius;
        [SerializeField] private string _interactionText;
        private ExplosionAnimControl _explosion;

        public override string showName => "Explosion Rune";

        public int radius
        {
            get => _radius;
            set => _radius = value;
        }

        public override void PlayAnimation(Animator animatorAura, Animator animatorZone)
        {
            animatorAura.Play(nameof(ExplosionRune));
            animatorZone.Play(nameof(ExplosionRune));
        }

        public override void ApplyEffect(Vector3 position, GridManager gridManager)
        {
            Vector2Int positionIndexes = gridManager.GetCellIndex(position);
            _explosion = Instantiate(_explosionPrefab, position, Quaternion.identity)
                .GetComponentInChildren<ExplosionAnimControl>();
            _explosion.SetSize(radius);
            _explosion.action += () =>
            {
                for (int x = positionIndexes.x - radius; x <= positionIndexes.x + radius; x++)
                for (int y = positionIndexes.y - radius; y <= positionIndexes.y + radius; y++)
                {
                    if (!gridManager.GetCellObjectsByType(x, y, out List<IExplosive> cellObjects)) continue;

                    foreach (IExplosive explosive in cellObjects) explosive.Explode();
                }
            };
            EventManager.instance.onRuneDropped?.Invoke();
        }
    }
}