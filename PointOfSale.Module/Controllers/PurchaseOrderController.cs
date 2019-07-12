using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using PointOfSale.Module.BusinessObjects;

namespace PointOfSale.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PurchaseOrderController : ViewController
    {
        public PurchaseOrderController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

            TargetObjectType = typeof(PurchaseOrder);


            SimpleAction markOrderAsDelivered = new SimpleAction(this, "moad", PredefinedCategory.RecordEdit);
            markOrderAsDelivered.TargetObjectType = typeof(PurchaseOrder);
            markOrderAsDelivered.TargetViewType = ViewType.ListView;
            markOrderAsDelivered.ConfirmationMessage = "";
            markOrderAsDelivered.Execute += MarkOrderAsDelivered_Execute;

            SimpleAction cancelOrder = new SimpleAction(this, "moad2", PredefinedCategory.RecordEdit);
            cancelOrder.TargetObjectType = typeof(PurchaseOrder);
            cancelOrder.TargetViewType = ViewType.DetailView;
            cancelOrder.ConfirmationMessage = "";
            cancelOrder.Execute += CancelOrder_Execute;



        }

        private void CancelOrder_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MarkOrderAsDelivered_Execute(object sender, SimpleActionExecuteEventArgs e)
        {

            var list = e.SelectedObjects;

            for(int j = 0;j < list.Count;j++)
            {
                PurchaseOrder po = (PurchaseOrder)list[j]  ;


            }


           
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
