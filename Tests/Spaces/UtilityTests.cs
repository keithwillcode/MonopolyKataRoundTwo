﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonopolyKata.Core.Spaces;
using MonopolyKata.Tests.Fakes;
using MonopolyKata.Core;
using MonopolyKata.Classic;
using System.Collections.Generic;

namespace MonopolyKata.Tests.Spaces
{
    [TestClass]
    public class UtilityTests
    {
       private String hat;
        private FakeDice fakeDice;
        private Utility electricCompany;
        private Utility waterWorks;

        public UtilityTests()
        {
            fakeDice = new FakeDice();            
            hat = "Hat";

            var banker = new Banker(new[] { hat });
            var utilityGroup = new List<Utility>();
            electricCompany = new Utility(ClassicBoardFactory.UtilityPrice, banker, utilityGroup, fakeDice);
            waterWorks = new Utility(ClassicBoardFactory.UtilityPrice, banker, utilityGroup, fakeDice);

            utilityGroup.Add(electricCompany);
            utilityGroup.Add(waterWorks);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            fakeDice.SetDieValues(1, 5);
            fakeDice.Roll();
        }

        [TestMethod]
        public void RentForOneOwnedUtilityIsFourTimesCurrentDiceValue()
        {
            electricCompany.Owner = hat;

            var rent = electricCompany.CalculateRent();

            Assert.AreEqual(4 * fakeDice.CurrentValue, rent);
        }

        [TestMethod]
        public void RentForTwoOwnedUtilitiesIsTenTimesCurrentDiceValue()
        {
            electricCompany.Owner = hat;
            waterWorks.Owner = hat;

            var rent = electricCompany.CalculateRent();

            Assert.AreEqual(10 * fakeDice.CurrentValue, rent);
        }
    }
}