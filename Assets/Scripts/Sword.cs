using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
[SerializeField] private Transform _blade;
[SerializeField] private Rigidbody _bladeRb;
[SerializeField] float _bladeLenght = 1.0f;

    //[SerializeField] private Transform _guard;
    private Vector3 _initialScale;
private Vector3 _targetScale;
private float _animationDuration = 0.1f;
private float _animationTime = 0f;
private bool _isAnimating = true;
//private Color _saberColor = Color.red;
//private Rigidbody _saber;
[SerializeField] private AudioSource _saberDraw;
[SerializeField] private AudioSource _saberSheathe;
[SerializeField] private AudioSource _saberActive;
//[SerializeField] private AudioSource _saberAttack;
//[SerializeField] private AudioSource _saberCollision;
void Start()
{       
     //_saberDraw.mute = true;
//_saber = GetComponent<Rigidbody>();
    _initialScale = _blade.localScale;
    _targetScale = _initialScale;
    _targetScale.y = _bladeLenght;    
}


void Update()
{

Debug.Log(_bladeRb.velocity);
    if (Input.GetButtonUp("Fire1"))
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