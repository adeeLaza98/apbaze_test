using Apbaze.StaticClasses;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Apbaze.ViewModels
{
    public class JobViewModel: INotifyPropertyChanged
    {
        #region Properties
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
        public string PostDate { get; set; }

        private bool alreadyProposed;

        public bool AlreadyProposed
        {
            get { return alreadyProposed; }
            set
            {
                if (alreadyProposed != value)
                {
                    alreadyProposed = value;
                    OnPropertyChanged(nameof(alreadyProposed));
                }
            }
        }

        private bool accepted;

        public bool Accepted
        {
            get { return accepted; }
            set
            {
                if (accepted != value)
                {
                    accepted = value;
                    OnPropertyChanged(nameof(Accepted));
                }
            }
        }

        private bool rejected;

        public bool Rejected
        {
            get { return rejected; }
            set
            {
                if (rejected != value)
                {
                    rejected = value;
                    OnPropertyChanged(nameof(Rejected));
                }
            }
        }

        private bool inReview;

        public bool InReview
        {
            get { return inReview; }
            set
            {
                if (inReview != value)
                {
                    inReview = value;
                    OnPropertyChanged(nameof(InReview));
                }
            }
        }

        private bool deliverableAccepted;

        public bool DeliverableAccepted
        {
            get { return deliverableAccepted; }
            set
            {
                if (deliverableAccepted != value)
                {
                    deliverableAccepted = value;
                    OnPropertyChanged(nameof(DeliverableAccepted));
                }
            }
        }

        private bool deliverableInReview;

        public bool DeliverableInReview
        {
            get { return deliverableInReview; }
            set
            {
                if (deliverableInReview != value)
                {
                    deliverableInReview = value;
                    OnPropertyChanged(nameof(DeliverableInReview));
                }
            }
        }

        private bool deliverableNotSent;

        public bool DeliverableNotSent
        {
            get { return deliverableNotSent; }
            set
            {
                if (deliverableNotSent != value)
                {
                    deliverableNotSent = value;
                    OnPropertyChanged(nameof(DeliverableNotSent));
                }
            }
        }

        private bool deliverableRejected;

        public bool DeliverableRejected
        {
            get { return deliverableRejected; }
            set
            {
                if (deliverableRejected != value)
                {
                    deliverableRejected = value;
                    OnPropertyChanged(nameof(DeliverableRejected));
                }
            }
        }

        private bool isNotCompleted;

        public bool IsNotCompleted
        {
            get { return isNotCompleted; }
            set
            {
                if (isNotCompleted != value)
                {
                    isNotCompleted = value;
                    OnPropertyChanged(nameof(isNotCompleted));
                }
            }
        }

        public string JobStatus { get; set; }

        public string ProposalsNumber { get; set; }

        public string FreelancerHired { get; set; }

        public string DeliverableStatus { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public JobViewModel(Job job)
        {
            AppBazeDataContext _context = new AppBazeDataContext();

            var jobBids = _context.Bids.Where(b => b.JobId == job.Id).ToList();
            var jobDeliverables = _context.Deliverables.Where(d => d.JobId == job.Id).ToList();

            Id = job.Id;
            Title = "#" + job.Id + " - " + job.Title;
            Description = job.Description;
            JobStatus = "Job status: " + (job.JobStatus == 1 ? "Open" : job.JobStatus == 2 ? "Hired" : "Completed");
            ProposalsNumber = jobBids.Count > 1 ? jobBids.Count.ToString() + " proposals" : jobBids.Count.ToString() + " proposal";

            var acceptedBid = jobBids.FirstOrDefault(b => b.BidStatus == 2);

            if (acceptedBid != null)
                FreelancerHired = "Freelancer: " + acceptedBid.User.FirstName + " " + acceptedBid.User.LastName;

            var jobBid = jobBids.FirstOrDefault(b => b.FreelancerId == UserContext.LoggedInUserId);

            if (jobBid != null)
            {
                InReview = jobBid.BidStatus == 1;
                Accepted = jobBid.BidStatus == 2;
                Rejected = jobBid.BidStatus == 3;
            }

            var myJobDeliverable = jobDeliverables.FirstOrDefault(d => d.FreelancerId == UserContext.LoggedInUserId);

            var jobDeliverable = jobDeliverables.OrderByDescending(d => d.CreatedAt).FirstOrDefault();

            if (myJobDeliverable != null)
            {
                DeliverableAccepted = myJobDeliverable.DeliverableStatus == 2;
                DeliverableInReview = myJobDeliverable.DeliverableStatus == 1;
                DeliverableRejected = myJobDeliverable.DeliverableStatus == 3;
            }
            else if (jobBid != null)
            {
                if (jobBid.BidStatus == 2)
                {
                    DeliverableStatus = "Deliverable status: Not sent";
                    DeliverableNotSent = true;
                }
            }

            if(jobDeliverable != null)
                DeliverableStatus = "Deliverable status: " + (jobDeliverable.DeliverableStatus == 1 ? "In review" : jobDeliverable.DeliverableStatus == 2 ? "Accepted" : "Rejected");
            else if(jobBid != null)
            {
                if (jobBid.BidStatus == 2)
                {
                    DeliverableStatus = "Deliverable status: Not sent";
                }
            }

            AlreadyProposed = jobBids.Any(b => b.FreelancerId == UserContext.LoggedInUserId);

            var paymentString = job.IsPaymentHourly ? "Hourly: $" + job.Price.ToString() : "Fixed price";
            var experienceString = job.ExperienceLevel == 1 ? "Beginner" : job.ExperienceLevel == 2 ? "Intermediate" : "Advanced";
            var estimateString = job.IsPaymentHourly ? (job.ProjectLength.HasValue ? "Est. Time: " + job.ProjectLength.Value.ToString() + (job.ProjectLength.Value > 1 ? " months" : " month") : string.Empty) : "Est. price: $" + job.Price.ToString();

            Details = paymentString + " - " + experienceString + (string.IsNullOrEmpty(estimateString) ? string.Empty : " - " + estimateString);

            TimeSpan timeDifference = DateTime.Now - job.CreatedAt;
            var hours = (int)timeDifference.TotalHours;

            PostDate = "Posted " + hours.ToString() + (hours > 1 ? " hours" : " hour") + " ago";

            IsNotCompleted = job.JobStatus != 3;
        }
    }
}
