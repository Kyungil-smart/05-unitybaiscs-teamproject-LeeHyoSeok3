using CartoonFX;
using System;
using UnityEngine;

public class BlockView : MonoBehaviour, IPoolable
{
    [SerializeField] private CFXR_Effect _destroyEffectPrefab;
    [SerializeField] private ScorePopup scorePopup;
    public void OnSpawn()
    {
        gameObject.SetActive(true);   
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnDespawn();
        }
    }

    public void OnDespawn()
    {
        if (_destroyEffectPrefab != null)
        {
            Instantiate(_destroyEffectPrefab, transform.position, Quaternion.identity);
        }
        gameObject.SetActive(false);
        if (scorePopup != null)
        {
            scorePopup.Show(100, transform);
        }
    }

    public void SetWorldPostion(Vector3 pos)
    {
        transform.position = pos;
    }
}