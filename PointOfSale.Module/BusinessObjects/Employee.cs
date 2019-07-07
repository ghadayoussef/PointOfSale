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
    [NavigationItem("Human Resources")]
    public class Employee : XPCustomObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public Employee(Session session)
            : base(session)
        {
        }
        [Key(AutoGenerate = true)]
        public int ID
        {
            get;
            set;
        }
        public String FirstName
        {
            get;
            set;
        }
        public String LastName
        {
            get;
            set;
        }
        public String JobTitle
        {
            get;
            set;
        }
        public float Salary
        {
            get;
            set;
        }
        private superviser _superviser;
        [Association("Superviser-Employees")]
        public superviser superviser
        {
            get
            {
                return _superviser;
            }
            set
            {

                SetPropertyValue("superviser", ref _superviser, value);
            }
        }

        public int SupervisorID
        {
            get
            {
                if (superviser != null)
                    return superviser.SuperviserID;
                else
                    return 0;

            }

        }
        public String PhoneNumber
        {
            get;
            set;
        }
        public String Gender
        {
            get;
            set;

        }
        public String Address
        {
            get;
            set;
        }
        public String Email
        {
            get;
            set;
        }
        [Association("Employee-Attendances")]
        public XPCollection<Attendance> Attendances
        {
            get
            {
                return GetCollection<Attendance>("Attendances");
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //private string _PersistentProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), Index(0), VisibleInListView(false)]
        //[Persistent("DatabaseColumnName"), RuleRequiredField(DefaultContexts.Save)]
        //public string PersistentProperty {
        //    get { return _PersistentProperty; }
        //    set { SetPropertyValue("PersistentProperty", ref _PersistentProperty, value); }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger a custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.PersistentProperty = "Paid";
        //}
    }
}