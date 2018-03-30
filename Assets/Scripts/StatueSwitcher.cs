using UnityEngine;

public class StatueSwitcher : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] GameObject[] _statues;

    #endregion

    #region Public members

    public bool enableRotation { get; set; }

    public void ChangeModel()
    {
        // Deactivate all.
        foreach (var go in _statues) go.SetActive(false);

        // Choose a statue.
        var next = _statues[Random.Range(0, _statues.Length)];

        // Reset the transform.
        next.transform.localPosition = Vector3.zero;
        if (!enableRotation) next.transform.localRotation = Quaternion.identity;

        // Then activate it.
        next.SetActive(true);

        // Terminate scrolling.
        _scrolling = false;
    }

    public void StartScroll()
    {
        // Deactivate all.
        foreach (var go in _statues) go.SetActive(false);

        // Change the state.
        _scrolling = true;
        _interval = 0;
    }

    #endregion

    #region Private members

    bool _scrolling;
    float _interval;

    GameObject ChooseFreeStatue()
    {
        for (var i = 0; i < 100; i++)
        {
            var go = _statues[Random.Range(0, _statues.Length)];
            if (!go.activeInHierarchy) return go;
        }

        Debug.LogError("Can't find a free statue.");
        return _statues[0]; // geez :(
    }

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        // Activate only the first statue.
        foreach (var go in _statues) go.SetActive(false);
        _statues[0].SetActive(true);
    }

    void Update()
    {
        if (_scrolling)
        {
            _interval -= Time.deltaTime;

            if (_interval <= 0)
            {
                // Choose the next statue and reset/activate it.
                var next = ChooseFreeStatue();
                next.transform.localPosition = Vector3.right * 5;
                next.SetActive(true);

                _interval = 2;
            }
        }

        // Deltas for animation
        var dt = Time.deltaTime;
        var dpos = Vector3.right * -dt;
        var drot = Quaternion.AngleAxis(dt * 90, Vector3.up);

        // Scrolling and animation
        foreach (var go in _statues)
        {
            if (!go.activeInHierarchy) continue;

            var tr = go.transform;
            if (_scrolling) tr.localPosition += dpos; 
            if (enableRotation) tr.localRotation = drot * tr.localRotation;

            if (tr.localPosition.x < -5) go.SetActive(false);
        }
    }

    #endregion
}
