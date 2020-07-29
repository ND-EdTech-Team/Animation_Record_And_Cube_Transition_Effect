using System.Linq;
using UnityEngine;

namespace Plugins.Cube.Physic
{
    public class BaseForce : MonoBehaviour
    {
        protected Rigidbody[] Targets { get; private set; }

        public void Initialize(GameObject[] targets)
        {
            Targets = targets.Select(t => t.GetComponent<Rigidbody>()).ToArray();
        }

        public virtual void AddForce()
        {
            foreach (var target in Targets)
                target.useGravity = true;
        }
    }
}