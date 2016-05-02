// unit tests for home controller ppr_web

using Microsoft.VisualStudio.TestTools.UnitTesting;
using ppr_web.Controllers;
using ppr_web.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace UnitTest
{
    [TestClass]
    public class ControllerUnitTest
    {
        public ControllerUnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        
        // test controller returns index view
        [TestMethod]
        public void Action_Should_Return_View_For_Index()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Index");
        }

        // test controller send email from contact page
        // success returns sent page
        [TestMethod]
        public async Task Action_Should_Send_An_Email_Contact()
        {
            // Arrange
            var controller = new HomeController();
            var contact = new ContactForm();
            contact.Comment = "test";
            contact.Email = "ward.keyth@gmail.com";
            contact.FirstName = "keyth";
            contact.LastName = "ward";

            // Act
            var result = await controller.Contact(contact) 
                as RedirectToRouteResult;

            // Assert
            Assert.AreEqual(result,"Sent");
        }

        // test controller method BlankEditorRow returns partial view and List object
        [TestMethod]
        public void Action_Should_Return_View_For_BlankEditorRow()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var result = controller.BlankEditorRow() as ViewResult;

            // Assert
            Assert.AreEqual(result.ViewName, 
                "~/Views/Shared/EditorTemplates/LineEditor.cshtml");
            Assert.IsInstanceOfType(result.ViewData.Model, typeof(Line));
        }

        // test controller returns instantiated object with view combined
        [TestMethod]
        public void Action_Should_Return_Object_With_View()
        {
            // Arrange
            var controller = new HomeController();
            var combined = new CombinedSearch();

            // Act
            var result = controller.Combined() as ViewResult;

            // Assert
            Assert.IsNotNull(result, "Combined");
            Assert.IsInstanceOfType(result.ViewData.Model, 
                typeof(CombinedSearch));
        }
    }
}
