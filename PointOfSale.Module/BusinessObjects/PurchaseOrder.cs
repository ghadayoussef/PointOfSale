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

namespace PointOfSale.Module.BusinessObjects
{
    [DefaultClassOptions]


    public class PurchaseOrder : XPCustomObject
    {
        public PurchaseOrder(Session session)
            : base(session)
        {

            //if(Number == 0)
            //    Date = DateTime.Now;

            if (session.IsNewObject(this))
            {
                Date = DateTime.Now;
                Status = OrderStatus.InProgress;
                //PaymentStatus = PaymentStatus.NotPaid;  
            }
        }



        [Key(AutoGenerate = true)]
        public int Number
        {
            get;
            set;
        }

        //Order Date
        private DateTime _date;
        [RuleRequiredField]
        [ModelDefault("EditMask", "dd-MM-yyy")]
        [ModelDefault("DisplayFormat", "dd-MM-yyy")]
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

        [XafDisplayName("Estimated Date")]
        [ModelDefault("EditMask", "dd-MM-yyy")]
        [ModelDefault("DisplayFormat", "dd-MM-yyy")]
        public DateTime ETD
        {
            get;
            set;
        }

        [XafDisplayName("Actual Delivery Date")]
        [ModelDefault("EditMask", "dd-MM-yyy")]
        [ModelDefault("DisplayFormat", "dd-MM-yyy")]
        public DateTime? DeliveryDate
        {
            get;
            set;
        }
        //[Association]

        private Supplier _supplier;
        [RuleRequiredField]
        [Association("Supplier-PurchaseOrders")]
        public Supplier Supplier
        {
            get
            {
                return _supplier;
            }
            set
            {
                SetPropertyValue("Supplier", ref _supplier, value);
            }

        }

        //[ModelDefault("AllowEdit","false")]
        [XafDisplayName("Status Of Items")]
        public OrderStatus Status
        {
            get;
            set;
        }

        //[ModelDefault("AllowEdit", "false")]
        [XafDisplayName("Status Of Payment")]
        public PaymentStatus PaymentStatus
        {
            get
            {
                if (DueAmount == 0)
                    return PaymentStatus.Paid;
                else
                    return PaymentStatus.NotPaid;
            }
            
        }



        [Browsable(false)]
        public int SupplierID

        {
            //get
            //{
            //    return Supplier.SupplierID;
            //}  
            get;
            set;
        }

        private decimal _orderTotal;
        [ModelDefault("EditMask", "f2")]
        [ModelDefault("DisplayFormat", "f2")]
        public decimal OrderTotal
        {
            get
            {
                _orderTotal = PoProducts.Sum(x => x.TotalLineAmount);
                return _orderTotal;
                //for(int i = 0; i < PoProducts.Count; i++)
                //{
                //    _orderTotal += PoProducts[i].TotalLineAmount;
                //}

            }
        }

        private decimal _paidAmount;
        [ImmediatePostData]
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
                //if(_paidAmount > 0 && _paidAmount < OrderTotal)
                //{
                //    PaymentStatus = PaymentStatus.PartiallyPaid;
                //}

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


        //[ModelDefault("AllowNew","true")]
        //[ModelDefault("AllowEdit","true")]
        //[ModelDefault("NewItemRowPosition","Bottom")]
        [Association("PurchaseOrder-PoProducts")]
        public XPCollection<PoProduct> PoProducts
        {
            get
            {
                return GetCollection<PoProduct>("PoProducts");
            }
        }

        [Action(AutoCommit = true,Caption ="Goods Delivered",
            ImageName = "BO_Product",
            ConfirmationMessage = "ASDFASDFSADFA",
            ToolTip = "This action increases item quantities in your inventory")]
        public void MarkOrderAsDelivered()
        {
            //Validation Code
            if (PoProducts.Any(x => x.Quantity <= 0 || x.Item == null || x.UnitPrice <= 0))
                throw new UserFriendlyException("Invalid Entries");
            else
            {
                Status = OrderStatus.GoodsDelivered;
                Logic.BusinessLogic.PurchasingLogic.UpdateItemQuantities(this);
                Session.Save(this);
                Session.CommitTransaction();

            }



        }

        //TODO: Why Save Button doesn't get disabled after commit
        [Action(AutoCommit = true,Caption = "Paid",ImageName = "BO_Invoice")]
        public void MarkAsPaid()
        {
            PaidAmount = DueAmount;
            Session.Save(this);
            Session.CommitTransaction();
        }

        [Action(AutoCommit = true, Caption = "Return Items", ImageName = "BO_Invoice")]
        public void CancelOrder()
        {
            Logic.BusinessLogic.PurchasingLogic.CancelPurchaseOrderItems(this, Session);
        }

        
          /// <summary>
          /// On Saving Event , when saving a Purchase Order and Setting the Order Status to GoodsDelivered
          /// Automatically Update Item available quantities by calling the method from Purchasing Class
          /// </summary>
        protected override void OnSaving()
        {
            base.OnSaving();

            //if(DeliveryDate.HasValue && DeliveryDate.Value != null)
            //{
            //}




        }
    
        /// <summary>
        /// After Object Saving, the OnSaved Event is triggered and you can implement your logic here
        /// </summary>
        protected override void OnSaved()
        {
            base.OnSaved();
        }

        /// <summary>
        /// OnDeleting Event, this event is called when the Delete Action is performed and you need to perform
        /// Specific logic
        /// </summary>
        protected override void OnDeleting()
        {
            base.OnDeleting();


        }


    }

    //Enumarator That shows the 
    public enum OrderStatus
    {
        InProgress = 1,
        GoodsDelivered = 2,
        Cancelled = 3,

    }

    public enum PaymentStatus
    {
        NotPaid = 0,
        Paid = 1,
        PartiallyPaid = 2,
    }
}