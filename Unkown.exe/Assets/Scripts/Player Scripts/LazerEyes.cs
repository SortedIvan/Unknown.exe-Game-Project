using System.Collections;

using System.Collections.Generic;

using UnityEngine;

public class LazerEyes : MonoBehaviour
{

    public float fireRate = 0;
    public float Damage = 5;
    public LayerMask whatToHit;
    public GameObject bulletPrefab;
    private float timeToFire = 0;
    Transform firePoint;
    // Start is called before the first frame update
    void Awake()
    {
        firePoint = transform.Find("MouthPoint");
        if (firePoint == null)
        {
            Debug.LogError("No FirePoint. What?");
        }
    }

    public float offset;

    void Update()
    {
        
        //Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

        if (fireRate == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        else
        {
            if (Input.GetMouseButton(0) && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / fireRate;
                Shoot();
            }
        }

    }
    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.blue);
        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);

        }
        GameObject clone = (GameObject)Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Destroy(clone, 1f);
    }
}