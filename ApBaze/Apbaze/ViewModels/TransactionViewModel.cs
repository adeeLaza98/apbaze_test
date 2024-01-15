using System;

namespace Apbaze.ViewModels
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        public string Amount { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Color { get; set; }
    }
}
