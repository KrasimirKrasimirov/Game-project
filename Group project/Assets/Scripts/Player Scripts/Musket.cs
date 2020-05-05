using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Musket : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    GameObject thePlayer;

    public static int currentAmmo;
    public int maxAmmo = 12;
    public static int loadedAmmo = 1;
    [SerializeField]
    float _reloadSpeed = 2.2f;
    [SerializeField]
    bool _canFire;
    bool _isReloading = false;

    public AudioClip reloadSound;
    public AudioClip shootSound;

    private AudioSource source;

    void Start()
    {
        _canFire = true;
        currentAmmo = maxAmmo;
        thePlayer = GameObject.Find("Player");
        source = thePlayer.GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        //PointToMouse();

        if (Input.GetMouseButtonDown(0))
        {
            if(loadedAmmo > 0)
            {
                if (_canFire == true)
                {
                    Debug.Log("---");
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
    //void PointToMouse()
    //{
    //    if (PauseMenu.isPaused != true)
    //    {
    //        Vector3 _mousePos = Input.mousePosition;
    //        _mousePos.z = 5.23f;

    //        Vector3 _objectPos = Camera.main.WorldToScreenPoint(transform.position);
    //        _mousePos.x = _mousePos.x - _objectPos.x;
    //        _mousePos.y = _mousePos.y - _objectPos.y;

    //        float _angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
    //        transform.rotation = Quaternion.Euler(new Vector3(0, 0, _angle));
    //    }
    //}

    //shoots the musket
    void Shoot()
    {
        if(currentAmmo > 0) { 
        _canFire = false;

           
            Debug.Log("fire");
            StartCoroutine(Stay());
        
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            //play firing anim
            //play firing sound
            source.PlayOneShot(shootSound);
            loadedAmmo--;
        StartCoroutine(WeaponReload());
        }
    }

    //waits an amount of seconds before allowing the weapon to be fired again
    IEnumerator WeaponReload()
    {
        _isReloading = true;
        _canFire = false;
       
        currentAmmo--;
        yield return new WaitForSeconds(_reloadSpeed);
        //play reload animation
        //play reload sounds
        source.PlayOneShot(reloadSound);

        loadedAmmo = 1;
        _canFire = true;
        _isReloading = false;
    }

    IEnumerator Stay()
    {
        Player player = thePlayer.GetComponent<Player>();
        player.myRigidbody.velocity = Vector2.zero;
        player.myRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        
        player.myAnimator.SetTrigger("Shoot");
        
        yield return new WaitForSeconds(0.5f);
        player.myRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }


}
