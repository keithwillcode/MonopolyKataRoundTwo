﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonopolyKata.Classic;
using MonopolyKata.Core.Board.Locations;
using MonopolyKata.Core;

namespace MonopolyKata.Tests.Board.Locations
{
    [TestClass]
    public class GoTests
    {
        private Player horse;
        private Go go;

        public GoTests()
        {
            horse = new Player("Horse");
            go = new Go(ClassicBoardFactory.GoLocation, ClassicGameConstants.GoSalaryBonus);
        }

        [TestMethod]
        public void PlayerLandingOnGoIncreasesPlayerBalanceByGoSalaryBonus()
        {
            var balanceBefore = horse.Balance;
            go.LandOn(horse);

            Assert.AreEqual(balanceBefore + ClassicGameConstants.GoSalaryBonus, horse.Balance);
        }
    }
}