/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using UnityEngine;
using System;

namespace Utilities.Components
{
    public class OptimizedScrollItem : MonoBehaviour, IComparable<OptimizedScrollItem>
    {
        protected int mIndex = -1;

        public int Index => mIndex;

        public int CompareTo(OptimizedScrollItem other)
        {
            return mIndex.CompareTo(other.mIndex);
        }

        public virtual void UpdateContent(int pIndex, bool pForce)
        {
            mIndex = pIndex;
        }

    }
}
