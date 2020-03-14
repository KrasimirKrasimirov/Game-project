using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;

    public static int currentAmmo;
    public static int maxAmmo = 24;
    public static int loadedAmmo = 1;
    [SerializeField]
    float _reloadSpeed = 3.2f;
    bool _canFire = true;
    bool _isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;    
    }
    // Update is called once per frame
    void Update()
    {
        //PointToMouse();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(loadedAmmo > 0)
            {
                if (_canFire == true)
                {
                    Shoot();
                }
            }
        }

        if(Input.GetButtonDown("Reload") && loadedAmmo <= 0)
        {
            if (currentAmmo > 0 && _isReloading == false)
            {
                StartCoroutine(WeaponReload());
            }
        }

    }

    //rotates the musket to point towards the mouse
    void PointToMouse()
    {
        if (PauseMenu.isPaused != true)
        {
            Vector3 _mousePos = Input.mousePosition;
            _mousePos.z = 5.23f;

            Vector3 _objectPos = Camera.main.WorldToScreenPoint(transform.position);
            _mousePos.x = _mousePos.x - _objectPos.x;
            _mousePos.y = _mousePos.y - _objectPos.y;

            float _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
        }
    }

    //shoots the musket
    void Shoot()
    {
        _canFire = false;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        //play firing anim
        //play firing sound
        loadedAmmo--;
        StartCoroutine(WeaponReload());
    }

    //waits an amount of seconds before allowing the weapon to be fired again
    IEnumerator WeaponReload()
    {
        _isReloading = true;
        Debug.Log("Reloading...");
        _canFire = false;
        //play reload animation
        //play reload sounds
        yield return new WaitForSeconds(_reloadSpeed);
        Debug.Log("Reload Over");
        currentAmmo--;
        loadedAmmo = 1;
        _canFire = true;
        _isReloading = false;
    }

}
