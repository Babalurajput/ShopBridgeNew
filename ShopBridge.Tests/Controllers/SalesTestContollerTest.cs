using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShopBridge.Controllers;
using ShopBridge.Models;
using ShopBridge.Tests.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShopBridge.Tests.Controllers
{
    [TestClass]
    public class SalesTestContollerTest
    {

        //[TestMethod]
        //public void IndexView()
        //{
        //    var salesController = GetItemController(new InmemoryItemRepository());
        //    ViewResult result = salesController.Index();
        //    Assert.AreEqual("Index", result.ViewName);
        //}
        private static SalesController GetItemController(IItemRepository Itemrepository)
        {
            SalesController Itemcontroller = new SalesController(Itemrepository);
            Itemcontroller.ControllerContext = new ControllerContext()
            {
                Controller = Itemcontroller,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return Itemcontroller;
        }

        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);

            public override IPrincipal User
            {
                get
                {
                    return _user;
                }
                set
                {
                    base.User = value;
                }
            }
        }

        [TestMethod]
        public void Create_PostItemInRepository()
        {
            InmemoryItemRepository itemrepository = new InmemoryItemRepository();
            SalesController empcontroller = GetItemController(itemrepository);
            Sale item = GetItemID();
            empcontroller.Create(item,null);
            IEnumerable<Sale> items = itemrepository.GetAllItem();
            Assert.IsTrue(items.Contains(item));
        }

        [TestMethod]
        public void Create_PostRedirectOnSuccess()
        {
            SalesController controller = GetItemController(new InmemoryItemRepository());
            Sale model = GetItemID();
            var result = (RedirectToRouteResult)controller.Create(model,null);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void ViewIsNotValid()
        {
            SalesController itemcontroller = GetItemController(new InmemoryItemRepository());
            itemcontroller.ModelState.AddModelError("", "mock error message");
            Sale model = GetItemName(1,"", "", 0,null);
            var result = (ViewResult)itemcontroller.Create(model,null);
            Assert.AreEqual("Create", result.ViewName);
        }

        /// <summary>  
        ///  
        /// </summary>  
        [TestMethod]
        public void RepositoryThrowsException()
        {
            // Arrange  
            InmemoryItemRepository itemrepository = new InmemoryItemRepository();
            Exception exception = new Exception();
            itemrepository.ExceptionToThrow = exception;
            SalesController controller = GetItemController(itemrepository);
            Sale item = GetItemID();
            var result = (ViewResult)controller.Create(item,null);
            Assert.AreEqual("Create", result.ViewName);
            ModelState modelState = result.ViewData.ModelState[""];
            Assert.IsNotNull(modelState);
            Assert.IsTrue(modelState.Errors.Any());
            Assert.AreEqual(exception, modelState.Errors[0].Exception);
        }

        Sale GetItemID()
        {
            return GetItemName(1,"Pen", "Pen", 10,null);
        }

        Sale GetItemName(int IntId,string StringName, string StringDescription, decimal DecPrice,byte[] byteImage)
        {
            return new Sale
            {
                Id = IntId,
                Name = StringName,
                Description = StringDescription,
                Price = DecPrice,
                Image = byteImage
            };
        }

        //[TestMethod]
        //public void GetAllEmployeeFromRepository()
        //{
        //    // Arrange  
        //    Sale sale1 = GetItemName(1, "Pen", "Pen", 10,null);
        //    Sale sale2 = GetItemName(2, "Pencil", "Pencil", 10,null);
        //    InmemoryItemRepository itemrepository = new InmemoryItemRepository();
        //    itemrepository.Add(sale1);
        //    itemrepository.Add(sale2);
        //    var controller = GetItemController(itemrepository);
        //    var result = controller.Index();
        //    var datamodel = (IEnumerable<Sale>)result.ViewData.Model;
        //    CollectionAssert.Contains(datamodel.ToList(), sale1);
        //    CollectionAssert.Contains(datamodel.ToList(), sale2);
        //}
    }
}