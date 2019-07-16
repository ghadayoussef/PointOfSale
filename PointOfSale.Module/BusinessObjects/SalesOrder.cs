using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace PointOfSale.Module.BusinessObjects
{
    [DefaultClassOptions]
    [Appearance("DueAmountRule",
        TargetItems = "DueAmount",
        Criteria = "DueAmount > 0",
        FontStyle = System.Drawing.FontStyle.Bold
        ,FontColor = "Red")]

    [Appearance("FinalizedOrder",
        TargetItems = "*",
        Criteria = "Status == 1",
        Enabled = false,
        FontStyle = System.Drawing.FontStyle.Bold)]
    
    public class SalesOrder : XPCustomObject
    { 
        public SalesOrder(Session session)
            : base(session)
        {
            if (session.IsNewObject(this))
            {
                Date = DateTime.Now;
                Status = SalesOrderStatus.InProgress;
            }
        }
        [Key(AutoGenerate = true)]
     
        public int OrderID
        {
            get;
            set;
        }
        private Customer _customer;
        [Association("Customer-SalesOrders")]
        public Customer Customer
        {
            get
            {
                return _customer;
            }
            set
            {
                SetPropertyValue("Customer", ref _customer, value);
            }
        }

        public int CustomerID
        {
            get
            {
                if(Customer != null)
                    return Customer.CustomerId;
                else
                    return 0;
            }
        }
        private DateTime _date;
        [RuleRequiredField]
        [ModelDefault("EditMask","dd-MM-yyyy")]
        [ModelDefault("DisplayFormat","dd-MM-yyyy")]
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                SetPropertyValue("Date", ref _date, value);
            }
        }
        public PaymentType PaymentType
        {
            get;
            set;
        }

        [ImmediatePostData]
        [ModelDefault("AllowEdit","false")]
        public SalesOrderStatus Status
        {
            get;
            set;
        }

        [Association("SalesOrders-SalesOProducts")]
        public XPCollection<SalesOProducts> SalesOProducts
        {
            get { return GetCollection<SalesOProducts>("SalesOProducts"); }
        }
        [ModelDefault("EditMask","f2")]
        [ModelDefault("DisplayFormat","f2")]
        private decimal _orderTotal;
        public decimal OrderTotal
        {
            get
            {
                _orderTotal = SalesOProducts.Sum(x => x.TotalLineAmount);
                return _orderTotal;
            }

        }
        private decimal _paidAmount;
        [ModelDefault("EditMask", "f2")]
        [ModelDefault("DisplayFormat", "f2")]
        public decimal PaidAmount
        {
            get
            {
                return _paidAmount;
            }
            set
            {
                SetPropertyValue("PaidAmount", ref _paidAmount, value);
            }
        }
        [ModelDefault("EditMask", "f2")]
        [ModelDefault("DisplayFormat", "f2")]
        public decimal DueAmount
        {
            get
            {
                return OrderTotal - PaidAmount;
            }
        }


        [XafDisplayName("Status Of Payment")]
        public PaymentStatus PaymentStatus
        {
            get
            {
                if (DueAmount == 0)
                    return PaymentStatus.Paid;
                else if (DueAmount < PaidAmount)
                    return PaymentStatus.PartiallyPaid;
                else 
                    return PaymentStatus.NotPaid;
            }

        }

        [Action(AutoCommit = true, 
            Caption = "Finalize Order",
            TargetObjectsCriteria = "Status = 0 && SalesOProducts.Count > 0",
           ImageName = "BO_Product",
           ConfirmationMessage = "Are you sure that you wish to perform this operation?",
           ToolTip = "This action decreases item quantities in your inventory")]
        public void FinalizeSalesOrder()
        {
            //Validation Code
            if (SalesOProducts.Any(x => x.Quantity <= 0 || x.Item == null || x.UnitPrice <= 0))
                throw new UserFriendlyException("Invalid Entries");
            else
            {
                Logic.BusinessLogic.SalesLogic.UpdateItemQuantityOnSale(this);
                Session.Save(this);
                Session.CommitTransaction();

            }
            
        }

    }

    public enum PaymentType
    {
        Cash = 0,
        Visa = 1,
    }
    
    public enum SalesOrderStatus
    {
        InProgress = 0,
        Finalized = 1,
        Returned = 2,
        Cancelled = 3,

    }
       

}