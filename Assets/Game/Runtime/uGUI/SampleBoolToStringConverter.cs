using Sim.Faciem.uGUI;
using UnityEngine;

namespace Game.Runtime.uGUI
{
    [CreateAssetMenu(fileName = "SampleBoolToStringConverter", menuName = "Converters/SampleBoolToStringConverter")]
    public class SampleBoolToStringConverter : SimConverter<bool, string>
    {
        public override string Convert(bool from)
        {
            return from ? "True" : "False";
        }
    }
}