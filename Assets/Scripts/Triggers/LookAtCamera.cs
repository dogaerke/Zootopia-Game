

using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private bool rotateAround;
    [SerializeField] private float rotateSpeed = 120f;
    [SerializeField] private Vector3 rotateAxis = Vector3.up;
    private Camera _camera;
    private Transform _transform;

    private void LateUpdate()
    {
           
        if (!_camera)
        {
            _camera = Camera.main;
        }

        if (!_transform) _transform = transform;

        _transform.LookAt(_transform.position + _camera.transform.rotation * Vector3.forward,
            _camera.transform.rotation * Vector3.up);

        if (rotateAround)
        {
            _transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
        }
    }
}
