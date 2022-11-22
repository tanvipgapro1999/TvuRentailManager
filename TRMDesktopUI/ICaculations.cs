using System.Collections.Generic;

namespace TRMDesktopUI
{
    public interface ICaculations
    {
        List<string> Register { get; set; }

        double Add(double x, double y);
    }
}