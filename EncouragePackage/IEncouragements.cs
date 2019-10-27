using System.Collections.Generic;
namespace IOIIIO.Discourage
{
    public interface IEncouragements
    {
        IEnumerable<string> AllEncouragements { get; set; }

        string GetRandomEncouragement();
    }
}