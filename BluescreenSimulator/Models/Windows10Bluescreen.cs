using Strings = BluescreenSimulator.Properties.Windows10BluescreenResources;
namespace BluescreenSimulator
{
    public class Windows10Bluescreen : BluescreenBase
    {
        public string Emoticon { get; set; } = Strings.Emoticon;

        public string MainText1 { get; set; } = Strings.MainText1;

        public string MainText2 { get; set; } = Strings.MainText2;

        public string Complete { get; set; } = Strings.Complete;

        public string MoreInfo { get; set; } = Strings.MoreInfo;

        public string SupportPerson { get; set; } = Strings.SupportPerson;

        public string StopCode { get; set; } = Strings.StopCode;

        public bool HideQR { get; set; } = false;

        public bool UseOriginalQR { get; set; } = true;
		
        public float TextDelay { get; set; }
    }
}
