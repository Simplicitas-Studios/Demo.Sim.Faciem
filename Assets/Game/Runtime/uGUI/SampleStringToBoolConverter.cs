using Sim.Faciem.uGUI;
using UnityEngine;

namespace Game.Runtime.uGUI
{
    [CreateAssetMenu(fileName = "SampleStringToBoolConverter", menuName = "Converters/SampleStringToBoolConverter")]
    public class SampleStringToBoolConverter : SimConverter<string, bool>
    {
        public override bool Convert(string from)
        {
            return bool.TryParse(from, out var result) && result;
        }
    }
}