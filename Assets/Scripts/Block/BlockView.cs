using CartoonFX;
using System;
using UnityEngine;

public class BlockView : MonoBehaviour, IPoolable
{
    [SerializeField] private CFXR_Effect _destroyEffectPrefab;
    [SerializeField] private ScorePopup scorePopup;
    public BlockControler Controler { get; private set; }
    
    private Outline _outline;
    private Rigidbody _rb;
    private Collider _cd;
    
    private Transform _follwTarget;
    private bool _isFollowing;
    
    public void Initialize(BlockControler controler) =>  Controler = controler;

    private void Awake()
    {
        _outline = GetComponent<Outline>();
        if(_outline != null) _outline.enabled = false;
        
        _rb = GetComponent<Rigidbody>();
        _cd = GetComponent<Collider>();
    }

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
    public void SetWorldPosition(Vector2Int pos)
    {
        transform.position = new Vector3(pos.x, transform.position.y, pos.y);
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
        _rb.velocity = Vector3.zero;
    }

    private void LateUpdate()
    {
        if (_isFollowing && _follwTarget != null)
        {
            transform.position = _follwTarget.position;
        }
    }

    public void ShowOutLine(Color color)
    {
        if (_outline == null) return;
        
        _outline.OutlineColor = color;
        _outline.enabled = true;
    }

    public void HideOutLine()
    {
        if (_outline == null) return;
        _outline.enabled = false;
    }

    // ------------------------
    // Block Colision Event
    // ------------------------

    private void OnCollisionEnter(Collision collision)
    {
        // 바닥과 충돌했을 때
        if (collision.gameObject.CompareTag("Floor"))
        {
            Controler.SetState(BlockState.Landed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 그리드와 충돌했을 때
        if (other.gameObject.CompareTag("Grid"))
        {
            GameEventBus.Raise(new GridUpdateEvent());
        }
    }

    public void EnableCollision()
    {
        _cd.enabled = true;
        _rb.isKinematic = false;
    }

    public void DisableCollision()
    {
        _cd.enabled = false;
        _rb.isKinematic = true;
    }
    
    
}