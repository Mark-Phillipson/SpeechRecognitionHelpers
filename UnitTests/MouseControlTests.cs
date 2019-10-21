using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class MouseControlTests
    {
        #region Contructor Tests

        [TestMethod]
        public void Test_constructor_shortArgs_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "5" });

            Assert.AreEqual(2, mc.Arguments.Count);
            Assert.AreEqual("5", mc.Arguments[0]);
            Assert.AreEqual("/1", mc.Arguments[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_constructor_Args_Fail()
        {
            var mc = new MouseControl.MouseControl(new string[] { "8", null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_constructor_NullArgs2_Fail()
        {
            var mc = new MouseControl.MouseControl(new string[] { null, "9" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_constructor_EmptyArgs_Fail()
        {
            var mc = new MouseControl.MouseControl(new string[] { "", "9" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_constructor_short_and_EmptyArgs_Fail()
        {
            var mc = new MouseControl.MouseControl(new string[] { "" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_constructor_short_and_NULLArgs_Fail()
        {
            var mc = new MouseControl.MouseControl(new string[] { null });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_constructor_short_delay_arg_Fail()
        {
            var mc = new MouseControl.MouseControl(new string[] { "0", "1" });
        }

        [TestMethod]
        public void Test_constructor_EmptyArgs_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[0] { });
            
            Assert.AreEqual(2, mc.Arguments.Count);
            Assert.AreEqual("/0", mc.Arguments[0]);
            Assert.AreEqual("/1", mc.Arguments[1]);
        }

        [TestMethod]
        public void Test_constructor_nullArgs_Pass()
        {
            var mc = new MouseControl.MouseControl(null);

            Assert.AreEqual(2, mc.Arguments.Count);
            Assert.AreEqual("/0", mc.Arguments[0]);
            Assert.AreEqual("/1", mc.Arguments[1]);
        }

        [TestMethod]
        public void Test_constructor_1arg_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "4" });

            Assert.AreEqual(2, mc.Arguments.Count);
            Assert.AreEqual("4", mc.Arguments[0]);
            Assert.AreEqual("/1", mc.Arguments[1]);
        }

        [TestMethod]
        public void Test_constructor_2args_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/2", "/6" });

            Assert.AreEqual(2, mc.Arguments.Count);
            Assert.AreEqual("/2", mc.Arguments[0]);
            Assert.AreEqual("/6", mc.Arguments[1]);
        }

        [TestMethod]
        public void Test_constructor_4args_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "10", "20", "30", "40" });

            Assert.AreEqual(4, mc.Arguments.Count);
            Assert.AreEqual("10", mc.Arguments[0]);
            Assert.AreEqual("20", mc.Arguments[1]);
            Assert.AreEqual("30", mc.Arguments[2]);
            Assert.AreEqual("40", mc.Arguments[3]);
        }

        #endregion

        #region Perform Control Tests

        [TestMethod]
        public void Test_PerformControl_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/0" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_missingArgs_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[0] { });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_missingArgs2_Pass()
        {
            var mc = new MouseControl.MouseControl(null);
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_missingArgs3_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1" });
            mc.PerformControl();
        }      

        [TestMethod]
        public void Test_PerformControl_right_click_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/right-click" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_upper_left_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/upper-left" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_uper_right_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/upper-right" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_lower_left_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/lower-left" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_right_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/right" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_left_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/left" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_down_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/down" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_up_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/up" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_click_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/click" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_double_click_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/double-click" });
            mc.PerformControl();
        }

        [TestMethod]
        public void Test_PerformControl_stop_Pass()
        {
            var mc = new MouseControl.MouseControl(new string[] { "/1", "/stop" });
            mc.PerformControl();
        }

        #endregion
    }
}
