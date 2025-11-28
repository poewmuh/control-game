using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class SimpleAnimate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _animationSprites;
    [SerializeField] private bool _randomStartSprite;
    [SerializeField] private int _settedStartSprite = 0;
    [SerializeField] private float _animationSpeed = 1;

    private void Start()
    {
        if (_randomStartSprite)
        {
            _settedStartSprite = Random.Range(0, _animationSprites.Length);
        }
        _spriteRenderer.sprite = _animationSprites[_settedStartSprite];
        StartAnimation(this.GetCancellationTokenOnDestroy()).Forget();
    }

    private async UniTaskVoid StartAnimation(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _spriteRenderer.sprite = _animationSprites[_settedStartSprite];
            await UniTask.Delay(TimeSpan.FromSeconds(_animationSpeed), cancellationToken: token);
            _settedStartSprite++;
            if (_settedStartSprite >= _animationSprites.Length)
            {
                _settedStartSprite = 0;
            }
        }
    }
}
