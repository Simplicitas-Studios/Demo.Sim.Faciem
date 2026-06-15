using Sim.Faciem.uGUI;
using TMPro;
using UnityEngine;

namespace Game.Runtime.uGUI
{
    [CreateAssetMenu(fileName = "SampleBoolToFontStylesConverter", menuName = "Converters/SampleBoolToFontStylesConverter")]
    public class SampleBoolToFontStylesConverter : SimConverter<bool, FontStyles>
    {
        public override FontStyles Convert(bool from)
        {
            return from ? FontStyles.Bold : FontStyles.Normal;
        }
    }
}