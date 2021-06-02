using AirportModels;
using System.Collections.ObjectModel;

namespace AirportSimulator2.AirportLayoutBuilder
{
    public interface IAirportBuilder
    {
        Collection<Leg> Build(int hangerMaxCapacity);
    }
}
