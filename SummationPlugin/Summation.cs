using Plugin;
using System.ComponentModel.Composition;

namespace SummationPlugin
{
    [Export(typeof(IPlugin))]
    public class Summation : IPlugin
    {
        public double Calculation(double leftValue, double rightValue)
        {
            return leftValue + rightValue;
        }
    }
}
