using UnityEngine;

public class StatueSwitcher : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject[] _statues;

    #endregion

    #region Public members

    public float scrollSpeed { get; set; }
    public float rotationSpeed { get; set; }

    public void Restart()
    {
        _scroll = 0;
        _rotation = 0;
    }

    public void Rehash()
    {
        _scroll = Random.Range(0, StatueCount);
    }

    #endregion

    #region Private members

    float _scroll;
    float _rotation;

    int StatueCount { get { return _statues.Length; } }

    #endregion

    #region MonoBehaviour implementation

    void Update()
    {
        _scroll += scrollSpeed * Time.deltaTime;
        _rotation += rotationSpeed * Time.deltaTime;

        var rot = Quaternion.AngleAxis(_rotation, Vector3.up);

        for (var i = 0; i < StatueCount; i++)
        {
            var go = _statues[i];

            var x = 5 - 2 * ((i + _scroll) % StatueCount + 0.5f);

            if (x > -5)
            {
                go.SetActive(true);
                var tr = go.transform;
                tr.localPosition = Vector3.right * x;
                tr.localRotation = rot;
            }
            else
            {
                go.SetActive(false);
            }
        }
    }

    #endregion
}
