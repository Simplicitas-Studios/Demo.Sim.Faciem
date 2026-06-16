using Sim.Faciem.Commands;

namespace Sim.Faciem.Demo.Editor
{
    public interface IDemoEditorWindowDataContext : IDataContext
    {
        Command NextView { get; set; }
        
        Command PreviousView { get; set; }
    }
}