using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialState
{
    Ripe,
    Unripe
}

public class Material : MonoBehaviour
{
    public SpriteRenderer _spirte = default;
    public Transform StopPosition = default;

    [SerializeField] private Sprite _unRipe;
    [SerializeField] private Sprite _ripe;
    [SerializeField] private float _ripeTime;

    private float _ripeCurrentTime = 0;
    private MaterialState _state = MaterialState.Ripe;

    private void Update()
    {
        if (_state == MaterialState.Unripe && _ripeCurrentTime <= 0)
        {
            _spirte.sprite = _ripe;
            _state = MaterialState.Ripe;
        }
        else if (_state == MaterialState.Unripe)
            _ripeCurrentTime -= Time.deltaTime;
    }

    public void Harvest()
    {
        _spirte.sprite = _unRipe;
        _state = MaterialState.Unripe;
        _ripeCurrentTime = _ripeTime;
    }
}
