namespace SpiceEngine.Scripting.Scripts
{
    public interface IScriptCompiler
    {
        void AddScript(Script script);
        void CompileScripts();
    }
}
