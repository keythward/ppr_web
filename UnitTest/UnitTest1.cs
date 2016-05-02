// unit tests on webjobworker 

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WebJobWorker.Data;
using WebJobWorker.Records;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        // test webjobworker role
        // test download file class - method Download()
        // should return string (temp filename contained downloaded file)
        // return null if unsuccessful
        public void TestMethodDownload()
        {
            // Arrange
            var download = new DownloadFile("2010");

            // Act
            var result = download.Download();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        // test webjobworker role
        // test download file class - method ConvertFile()
        // should convert csv file and return List<Record>
        public void TestMethodConvertFile()
        {
            // Arrange
            var download = new DownloadFile("2010");
            var fileName= download.Download();

            // Act
            var result = download.ConvertFile(fileName);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count>0);
        }

        [TestMethod]
        // test webjobworker role
        // test download file class - method CleanRecords()
        // should convert List<Record> to List<AlteredRecord>
        // this method cleans and sorts the data in the given list 
        // before being placed in database
        public void TestMethodCleanRecords()
        {
            // Arrange
            var download = new DownloadFile("2010");
            var fileName = download.Download();
            var startList= download.ConvertFile(fileName);
            var cleanRecordsClass = new CleanUpRecords();

            // Act
            var result = cleanRecordsClass.CleanRecords(startList);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            Assert.IsInstanceOfType(result,
                typeof(List<AlteredRecord>));
        }


    }
}
