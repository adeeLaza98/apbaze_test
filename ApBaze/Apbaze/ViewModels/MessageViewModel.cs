namespace Apbaze.ViewModels
{
    public class MessageViewModel
    {
        public string Text { get; set; }
        public int Column { get; set; }
        public string Alignment { get; set; }
        public string Background { get; set; }
        public bool Sent { get; set; }
        public bool Received { get; set; }
    }
}
