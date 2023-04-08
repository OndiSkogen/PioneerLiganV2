namespace PioneerLigan.HelperClasses
{
    public class FolderInfo
    {
        public string Path { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<PageInfo> Files { get; set; } = new List<PageInfo>();
    }
}
