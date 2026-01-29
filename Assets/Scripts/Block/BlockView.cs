using CartoonFX;
using System;
using UnityEngine;

public class BlockView : MonoBehaviour, IPoolable
{
    [SerializeField] private CFXR_Effect _destroyEffectPrefab;
    [SerializeField] private ScorePopup scorePopup;
    public BlockControler Controler { get; private set; }
    
    private Transform _follwTarget;
    private bool _isFollowing;
    
    public void Initialize(BlockControler controler) =>  Controler = controler;
    
    public void OnSpawn()
    {
        gameObject.SetActive(true);   
    }

    private void Update()
    {
        // 테스트용: D키 누르면 박스 비활성화
        // if (Input.GetKeyDown(KeyCode.D))
        // {
        //     OnDespawn();
        // }
    }

    public void OnDespawn()
    {
        // 이펙트 표출하는 기능
        if (_destroyEffectPrefab != null)
        {
            Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity);
        }
        // 박스 비활성화
        gameObject.SetActive(false);
        // 점수 표출하는 기능
        if (scorePopup != null)
        {
            scorePopup.Show(100, transform);
        }
    }
    
    public void SetWorldPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    public void AttachTo(Transform target)
    {
        _follwTarget = target;
        _isFollowing = true;
    }

    public void Detach()
    {
        _isFollowing = false;
        _follwTarget = null;
    }

    private void LateUpdate()
    {
        if (_isFollowing && _follwTarget != null)
        {
            transform.position = _follwTarget.position;
        }
    }
    
    
}