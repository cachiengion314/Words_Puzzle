/**
 * Author NBear - nbhung71711@gmail.com - 2017
 **/

using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

namespace Utilities.Components
{
    [AddComponentMenu("Utitlies/UI/TextFormat")]
    public class TextFormat : MonoBehaviour
    {
        public enum FormatType
        {
            UPPERCASE,
            lowercase,
            CapitalizeEachWord,
            Sentence_case
        }

        public FormatType formatType;

        private void OnEnable()
        {
            Format();
        }

        public void Format()
        {
            Text text = gameObject.FindComponentInChildren<Text>();
            switch (formatType)
            {
                case FormatType.UPPERCASE:
                    text.text = text.text.ToUpper();
                    break;

                case FormatType.Sentence_case:
                    text.text = text.text.ToSentenceCase();
                    break;

                case FormatType.lowercase:
                    text.text = text.text.ToLower();
                    break;

                case FormatType.CapitalizeEachWord:
                    text.text = text.text.ToCapitalizeEachWord();
                    break;
            }
        }

        public void FormatAllChilren()
        {
            var components = gameObject.FindComponentsInChildren<Text>();
            foreach(var t in components)
            {
                switch (formatType)
                {
                    case FormatType.UPPERCASE:
                        t.text = t.text.ToUpper();
                        break;

                    case FormatType.Sentence_case:
                        t.text = t.text.ToSentenceCase();
                        break;

                    case FormatType.lowercase:
                        t.text = t.text.ToLower();
                        break;

                    case FormatType.CapitalizeEachWord:
                        t.text = t.text.ToCapitalizeEachWord();
                        break;
                }
            }
        }
    }
}