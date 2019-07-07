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

            if(Number == 0)
                DeliveryDate = DateTime.Now;

            if (session.IsNewObject(this))
                DeliveryDate = DateTime.Now;




        }
       
        

        [Key(AutoGenerate = true)]
        public int Number
        {
        get;
        set;
        }
        public DateTime DeliveryDate {
            get;
            set;
        }
        //[Association]

        private Supplier _supplier;
        [Browsable(false)]
        [Association("Supplier-PurchaseOrders")]
        public Supplier Supplier
        {
            get
            {
                return _supplier;
            }
            set
            {
                SetPropertyValue("Supplier",ref _supplier, value);
            }
            
        }

        public int SupplierID
            
        {
            //get
            //{
            //    return Supplier.SupplierID;
            //}  
            get;
            set;
        }
        [Association("PurchaseOrder-PoProducts")]
        public XPCollection<PoProduct> PoProducts
        {
            get { 
return GetCollection<PoProduct>("PoProducts");
            }
        }


        protected override void OnSaving()
        {
            base.OnSaving();

            


        }

        protected override void OnDeleting()
        {
            base.OnDeleting();


        }


    }
}