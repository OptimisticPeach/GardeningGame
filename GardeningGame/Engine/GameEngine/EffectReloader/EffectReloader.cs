using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System.IO;
using System.Threading;

namespace GardeningGame.Engine.Scenes.Game
{
    public delegate void EffectChangedEvent(Effect E);
    public class EffectReloader
    {
        public event EffectChangedEvent OnEffectChanged;

        public FileSystemWatcher FSW;

        string Path;
        string FileName;

        public GraphicsDevice Device;

        public EffectReloader(string Path, string FileName, GraphicsDevice Device)
        {
            this.Path = Path;
            this.FileName = FileName;
            this.Device = Device;
            FSW = new FileSystemWatcher(Path, "*.fx");
            FSW.Changed += RecompileEffect;
            FSW.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            FSW.EnableRaisingEvents = true;
        }

        public void RecompileEffect(object sender, FileSystemEventArgs e)
        {
            if (e.Name == FileName)
            {
                MonoGame.Framework.Content.Pipeline.Builder.PipelineManager PM = new MonoGame.Framework.Content.Pipeline.Builder.PipelineManager(Path, ".\\tempBin", ".\\tempDir");
                bool Worked = false;
                string DebugStream = "";
                while (!Worked)
                    try
                    {
                        var BuiltContent = PM.BuildContent(Path + "\\" + FileName);
                        DebugStream += "BuildContent - Okay\n";
                        var ProcessedContent = PM.ProcessContent(BuiltContent);
                        DebugStream += "ProcessContent - Okay\n";
                        OnEffectChanged?.Invoke(new Effect(Device, ((CompiledEffectContent)ProcessedContent).GetEffectCode()));
                        DebugStream += "Invoked Creation - Okay\n";
                        Worked = true;
                        File.Delete(".\\tempBin\\" + FileName.TrimEnd(".fx".ToArray()) + ".xnb");
                        DebugStream += "Deleted File - Okay\n";
                    }
                    catch (InvalidContentException E)
                    {
                        Common.Debug.DebugConsole?.WriteLine("CompilerException");
                        Common.Debug.DebugConsole?.WriteLine(E.Message);
                        Common.Debug.DebugConsole?.WriteLine("Debug message: \n" + DebugStream);
                        Worked = true;
                    }
                    catch (IOException E)
                    {
                        Common.Debug.DebugConsole?.WriteLine("Most likely a leftover file when exiting. Check .\\tempBin");
                        Common.Debug.DebugConsole?.WriteLine(E.Message);
                        Common.Debug.DebugConsole?.WriteLine("Debug message: \n" + DebugStream);
                        continue;
                    }
                    catch (PipelineException E)
                    {
                        Common.Debug.DebugConsole?.WriteLine("Pipeline exception!");
                        Common.Debug.DebugConsole?.WriteLine(E.Message);
                    } 
                    catch (Exception E)
                    {
                        Common.Debug.DebugConsole?.WriteLine(E.Message);
                        Common.Debug.DebugConsole?.WriteLine("Debug message: \n" + DebugStream);
                        continue;
                    }
            }
        }

        private string ReadAllText(string Filename)
        {
            using (var stream = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var SR = new StreamReader(stream))
            {
                return SR.ReadToEnd();
            }
        }
    }
    class MyImporterContext : ContentImporterContext
    {
        public override string IntermediateDirectory { get { return string.Empty; } }
        public override string OutputDirectory { get { return string.Empty; } }

        public override ContentBuildLogger Logger { get { return logger; } }
        ContentBuildLogger logger = new MyLogger();

        public override void AddDependency(string filename) { }
    }
    class MyProcessorContext : ContentProcessorContext
    {
        public override TargetPlatform TargetPlatform { get { return TargetPlatform.Windows; } }
        public override GraphicsProfile TargetProfile { get { return GraphicsProfile.Reach; } }
        public override string BuildConfiguration { get { return string.Empty; } }
        public override string IntermediateDirectory { get { return string.Empty; } }
        public override string OutputDirectory { get { return string.Empty; } }
        public override string OutputFilename { get { return string.Empty; } }

        public override OpaqueDataDictionary Parameters { get { return parameters; } }
        OpaqueDataDictionary parameters = new OpaqueDataDictionary();

        public override ContentBuildLogger Logger { get { return logger; } }

        public override ContentIdentity SourceIdentity => throw new NotImplementedException();

        ContentBuildLogger logger = new MyLogger();

        public override void AddDependency(string filename) { }
        public override void AddOutputFile(string filename) { }

        public override TOutput Convert<TInput, TOutput>(TInput input, string processorName, OpaqueDataDictionary processorParameters) { throw new NotImplementedException(); }
        public override TOutput BuildAndLoadAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName) { throw new NotImplementedException(); }
        public override ExternalReference<TOutput> BuildAsset<TInput, TOutput>(ExternalReference<TInput> sourceAsset, string processorName, OpaqueDataDictionary processorParameters, string importerName, string assetName) { throw new NotImplementedException(); }
    }
    class MyLogger : ContentBuildLogger
    {
        public override void LogMessage(string message, params object[] messageArgs) { }
        public override void LogImportantMessage(string message, params object[] messageArgs) { }
        public override void LogWarning(string helpLink, ContentIdentity contentIdentity, string message, params object[] messageArgs) { }
    }
}
