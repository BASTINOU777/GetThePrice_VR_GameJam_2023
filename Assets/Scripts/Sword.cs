using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
[SerializeField] private Transform _blade;
//[SerializeField] private Transform _guard;
private Vector3 _initialScale;
private Vector3 _targetScale;
private float _animationDuration = 0.1f;
private float _animationTime = 0f;
private bool _isAnimating = false;
private Color _saberColor = Color.red;
private float _bladeLenght = 0.3f;
[SerializeField] private AudioSource _saberDraw;
[SerializeField] private AudioSource _saberSheathe;
[SerializeField] private AudioSource _saberActive;
void Start()
{
    //mute the audio sources
    _saberDraw.mute = true;
    _initialScale = _blade.localScale;
    _targetScale = _initialScale;
    _targetScale.y = _bladeLenght;
    _blade.GetComponent<Renderer>().material.color = _saberColor;
}


void Update()
{

    if (Input.GetMouseButtonDown(0))
    {
        if (!_isAnimating  )
        {
            _isAnimating = true;
        }
    }

    if (_isAnimating)
    {
        
       
        _animationTime += Time.deltaTime;
        float t = _animationTime / _animationDuration;
        _blade.localScale = Vector3.Lerp(_initialScale, _targetScale, t);
        if (_animationTime >= _animationDuration)
        {
            if(_targetScale.y == _bladeLenght)
            {
                _saberDraw.mute = false;
                _saberActive.mute = false;
                _saberActive.loop = true;
                _saberActive.Play();
                _saberDraw.Play();
                _saberSheathe.mute = true;

                
            }
            else
            {
                _saberSheathe.mute = false;
                _saberSheathe.Play();
                _saberActive.mute = true;
                _saberDraw.mute = true;
            }
            
            
            _isAnimating = false;
            _animationTime = 0f;
            Vector3 temp = _initialScale;
            _initialScale = _targetScale;
            _targetScale = temp;
        }
    }
}
}