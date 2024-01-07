using System;

namespace Apbaze.ViewModels
{
    public class JobViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string PostDate { get; set; }

        public JobViewModel(Job job)
        {
            Title = job.Title;
            Description = job.Description;

            var paymentString = job.IsPaymentHourly ? "Hourly: $" + job.Price.ToString() : "Fixed price";
            var experienceString = job.ExperienceLevel == 1 ? "Beginner" : job.ExperienceLevel == 2 ? "Intermediate" : "Advanced";
            var estimateString = job.IsPaymentHourly ? (job.ProjectLength.HasValue ? "Est. Time: " + job.ProjectLength.Value.ToString() + (job.ProjectLength.Value > 1 ? " months" : " month") : string.Empty) : "Est. price: $" + job.Price.ToString();

            Details = paymentString + " - " + experienceString + (string.IsNullOrEmpty(estimateString) ? string.Empty : " - " + estimateString);

            var hours = (DateTime.Now - job.CreatedAt).Hours;

            PostDate = "Posted " + hours.ToString() + (hours > 1 ? " hours" : " hour") + " ago";
        }
    }
}
