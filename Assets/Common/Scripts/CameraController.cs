using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera _targetCamera;
    [SerializeField] PostProcessVolume _postProcess;

    [SerializeField, Range(0, 1)] float _parameter;

    public float parameter {
        get { return _parameter; }
        set { _parameter = value; }
    }

    void LateUpdate()
    {
        var fov = Mathf.Lerp(11, 120, _parameter);

        var tan =
            Mathf.Tan(11 * Mathf.Deg2Rad / 2) /
            Mathf.Tan(fov * Mathf.Deg2Rad / 2);

        var dist = tan * 10 - 0.2f * _parameter;
        var pos = Vector3.forward * -dist;

        _targetCamera.transform.parent.parent.localPosition = pos;
        _targetCamera.fieldOfView = fov;

        _postProcess.weight = _parameter;
    }
}
