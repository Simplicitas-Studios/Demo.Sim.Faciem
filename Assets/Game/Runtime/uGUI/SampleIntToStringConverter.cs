using Sim.Faciem.uGUI;
using UnityEngine;

namespace Game.Runtime.uGUI
{
    [CreateAssetMenu(fileName = "SampleIntToStringConverter", menuName = "Converters/SampleIntToStringConverter")]
    public class SampleIntToStringConverter : SimConverter<int, string>
    {
        public override string Convert(int from)
        {
            return from switch
            {
                0 => "0",
                1 => "1",
                2 => "2",
                _ => "any"
            };
        }
    }
}