using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EscrowProofOfConcept.Models
{
    public class EscrowViewModel
    {
        public int id { get; set; }
        public string buyer { get; set; }
        public string seller { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public double itemsAmount { get; set; }
        public DateTime creationDate { get; set; }
        public bool isTransactionCreated { get; set; }
        public bool isBuyerAgree { get; set; }
        public bool isSellerAgree { get; set; }
        public bool isBuyerPay { get; set; } 
        public bool isDisbursed { get; set; }
        public bool isApprovePayment { get; set; }
        public bool isAvailablePaymentOptions { get; set; }
        public bool isProcessComplete { get; set; }
        public bool isSellerDeliverItems { get; set; }
        public bool isBuyerRecieveItems { get; set; }
        public bool isBuyerAcceptMilestone { get; set; }
        public List<Item> items { get; set; }
        public AvailableOptions availableOptions{ get; set; }

    }

    public class ExtraAttributes
    {
        public bool concierge { get; set; }
        public bool with_content { get; set; }
    }

    public class Fee
    {
        public string amount { get; set; }
        public string payer_customer { get; set; }
        public string type { get; set; }
    }

    public class Status
    {
        public bool secured { get; set; }
        public bool accepted { get; set; }
        public bool accepted_returned { get; set; }
        public bool received { get; set; }
        public bool received_returned { get; set; }
        public bool rejected { get; set; }
        public bool rejected_returned { get; set; }
        public bool shipped { get; set; }
        public bool shipped_returned { get; set; }
    }

    public class Schedule
    {
        public double amount { get; set; }
        public string beneficiary_customer { get; set; }
        public string payer_customer { get; set; }
        public Status status { get; set; }
    }

    public class Item
    {
        public string description { get; set; }
        public ExtraAttributes extra_attributes { get; set; }
        public List<Fee> fees { get; set; }
        public int id { get; set; }
        public int inspection_period { get; set; }
        public int quantity { get; set; }
        public List<Schedule> schedule { get; set; }
        public Status status { get; set; }
        public string title { get; set; }
        public string type { get; set; }
    }

    public class Party
    {
        public bool agreed { get; set; }
        public string customer { get; set; }
        public string role { get; set; }
    }

    public class EscrowRootModel
    {
        public DateTime creation_date { get; set; }
        public string currency { get; set; }
        public string description { get; set; }
        public int id { get; set; }
        public List<Item> items { get; set; }
        public List<Party> parties { get; set; }

        public bool isProcessComplete { get; set; }
    }



    public class AvailablePaymentMethod
    {
        public string type { get; set; }
        public double total { get; set; }
        public string currency { get; set; }
    }

    public class AvailableOptions
    {
        public List<AvailablePaymentMethod> available_payment_methods { get; set; }
    }


}