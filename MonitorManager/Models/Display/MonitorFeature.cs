namespace MonitorManager.Models.Display
{
    public struct MonitorFeature
    {
        public bool Supported;
        public uint Min, Max, Current, Original;
        public string Capabilities; 
    }
}
