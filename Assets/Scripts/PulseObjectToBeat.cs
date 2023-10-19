using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseObjectToBeat : MonoBehaviour
{
    [SerializeField] bool _useTestBeat;
    [SerializeField] float _pulseSize = 0.95f;
    [SerializeField] float _returnSpeed = 5f;
    //private float _startSize;
    //private Camera _camera;
    private Vector3 _startSize;

    // Start is called before the first frame update
    void Start()
    {
        //_camera = GetComponent<Camera>();
        //_startSize = _camera.orthographicSize;
        _startSize = transform.localScale;
        if (_useTestBeat)
        {
            StartCoroutine(TestBeat());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //_camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _startSize, Time.deltaTime * _returnSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, _startSize, Time.deltaTime * _returnSpeed);
    }

    public void Pulse()
    {
        //_camera.orthographicSize = _startSize * _pulseSize;
        transform.localScale = _startSize * _pulseSize;
    }

    IEnumerator TestBeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Pulse();
        }
    }
}
