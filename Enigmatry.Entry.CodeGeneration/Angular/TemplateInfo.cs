namespace Enigmatry.Entry.CodeGeneration.Angular
{
    public class TemplateInfo
    {
        public string TemplatePath { get; set; }
        public string FileNamingPattern { get; set; }

        public TemplateInfo(string templatePath, string fileNamingPattern)
        {
            TemplatePath = templatePath;
            FileNamingPattern = fileNamingPattern;
        }
    }
}
