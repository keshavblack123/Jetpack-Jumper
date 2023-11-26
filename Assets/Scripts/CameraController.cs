using UnityEngine;
using DG.Tweening;
using System;

public class CameraController : MonoBehaviour
{
    //private Transform player;

    //private Vector3 offset = new Vector3(0f, 0f, -10f);
    //public float smoothTime = 0.125f;
    //private Vector3 velocity = Vector3.zero;

    //// Start is called before the first frame update
    //void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    Vector3 targetPosition = player.position + offset;
    //    transform.DOMove(targetPosition, smoothTime).SetEase(Ease.InOutQuad);
    //}

    //public void ResetCamera() { }

    // WithoutDoTween
    private Transform player;
    public float leftLimit;
    public float rightLimit;

    private Vector3 offset = new Vector3(0f, 0f, -10f);
    public float smoothTime = 0.125f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = player.position + offset;
        targetPosition.y = Mathf.Clamp(targetPosition.y, 5f, Mathf.Infinity);

        targetPosition.x = Mathf.Clamp(targetPosition.x, leftLimit, rightLimit);


        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
