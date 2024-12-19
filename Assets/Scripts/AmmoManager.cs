using System.Collections;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private int _maxAmmo = 30;
    [SerializeField] private float _reloadTime = 2f;

    private int _currentAmmo;
    private bool _isReloading = false;

    public int CurrentAmmo => _currentAmmo;
    public int MaxAmmo => _maxAmmo;
    public bool IsReloading => _isReloading;

    public delegate void AmmoEvent();
    public event AmmoEvent OnAmmoChanged;
    public event AmmoEvent OnReloadStart;
    public event AmmoEvent OnReloadComplete;

    private void Start()
    {
        _currentAmmo = _maxAmmo;
        GameManager.Instance.UpdateAmmoUI(_currentAmmo, _maxAmmo);
        OnAmmoChanged?.Invoke();
    }

    public bool TryShoot()
    {
        if (_isReloading || _currentAmmo <= 0)
            return false;

        _currentAmmo--;
        OnAmmoChanged?.Invoke();

        if (_currentAmmo <= 0)
            Reload();

        return true;
    }

    public void Reload()
    {
        if (_isReloading) return;
        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        OnReloadStart?.Invoke();
        yield return new WaitForSeconds(_reloadTime);
        _currentAmmo = _maxAmmo;
        _isReloading = false;
        OnReloadComplete?.Invoke();
    }
}
