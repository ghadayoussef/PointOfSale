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
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class SalesOrder : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public SalesOrder(Session session)
            : base(session)
        {
            if (session.IsNewObject(this))
            {
                Date = DateTime.Now;
                Status = OrderStatus.InProgress;
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
                return Customer.CustomerId;
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
        public String PaymentType
        {
            get;
            set;
        }
        [XafDisplayName("Status of the order")]
        public OrderStatus Status
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
    }
       

}