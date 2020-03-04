/**
 * Author NBear - nbhung71711 @gmail.com - 2017
 **/

using UnityEngine;

namespace Utilities.Common
{
    public static class VectorExtension
    {
        public static bool InsideBounds(this Vector2 pPosition, Bounds pBounds)
        {
            if (pPosition.x < pBounds.min.x)
                return false;
            if (pPosition.x > pBounds.max.x)
                return false;
            if (pPosition.y < pBounds.min.y)
                return false;
            if (pPosition.y > pBounds.max.y)
                return false;
            return true;
        }
    }
}
