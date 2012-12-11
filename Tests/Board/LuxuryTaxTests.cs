﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonopolyKata.Classic;
using MonopolyKata.Core.Board.Locations;
using MonopolyKata.Core;

namespace MonopolyKata.Tests.Board
{
    [TestClass]
    public class LuxuryTaxTests
    {
        private Player horse;
        private LuxuryTax luxuryTax;

        public LuxuryTaxTests()
        {
            luxuryTax = new LuxuryTax(ClassicBoardFactory.LuxuryTaxLocation, ClassicGameConstants.LuxuryTaxPaymentAmount);
        }

        [TestMethod]
        public void PlayerLandingOnLuxuryTaxDecreasesPlayerBalanceByLuxuryTaxAmount()
        {
            horse = new Player("Horse", 1500);
            luxuryTax.LandOn(horse);

            Assert.AreEqual(1500 - ClassicGameConstants.LuxuryTaxPaymentAmount, horse.Balance);
        }
    }
}