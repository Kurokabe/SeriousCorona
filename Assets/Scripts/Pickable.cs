using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SeriousCorona
{
    public class Pickable : MonoBehaviour
    {
        public void Pick()
        {
            print($"You picked {name}");
            Destroy(gameObject);
        }
    }
}
