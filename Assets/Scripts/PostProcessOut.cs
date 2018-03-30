using UnityEngine;
using Klak.Wiring;
using UnityEngine.Rendering.PostProcessing;

namespace Museum
{
    [AddComponentMenu("Klak/Wiring/Output/Component/Post Process Out")]
    public class PostProcessOut : NodeBase
    {
        #region Editable properties

        [SerializeField] PostProcessVolume _volume;

        #endregion

        #region Node I/O

        [Inlet]
        public float weight {
            set {
                Debug.Log(_volume);
                if (!enabled || _volume == null) return;
                _volume.weight = value;
            }
        }

        #endregion
    }
}
