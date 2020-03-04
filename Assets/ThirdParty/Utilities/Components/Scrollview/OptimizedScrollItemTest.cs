/**
 * Author NBear - nbhung71711@gmail.com - 2017
 **/

 using UnityEngine.UI;

namespace Utilities.Components
{
    public class OptimizedScrollItemTest : OptimizedScrollItem
    {
        public Text mTxtIndex;

        public override void UpdateContent(int pIndex, bool pForce)
        {
            base.UpdateContent(pIndex, pForce);

            name = pIndex.ToString();
            mTxtIndex.text = pIndex.ToString();
        }
    }
}