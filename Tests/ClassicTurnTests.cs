﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonopolyKata.Classic;
using MonopolyKata.Core;
using MonopolyKata.Core.Spaces;
using MonopolyKata.Core.Rules;
using System;
using Moq;

namespace MonopolyKata.Tests
{
    [TestClass]
    public class ClassicTurnTests
    {
        private Mock<Dice> mockDice;
        private Queue<Int32> dieValues;
        private String horse;
        private Board board;
        private ClassicTurn turn;
        private IEnumerable<Property> properties;
        private Banker banker;

        [TestInitialize]
        public void Initialize()
        {
            mockDice = new Mock<Dice>();
            mockDice.Setup(m => m.RollDie()).Returns(() => dieValues.Dequeue());
            horse = "Horse";

            banker = new Banker(new[] { horse });

            var propertyManager = new PropertyManager();
            properties = ClassicBoardFactory.CreateProperties(banker, propertyManager);
            propertyManager.ManageProperties(properties);
            
            board = ClassicBoardFactory.CreateBoard(mockDice.Object, Enumerable.Empty<IMovementRule>(), properties, banker, new[] { horse });
            turn = new ClassicTurn(mockDice.Object, board, banker, propertyManager);
        }

        [TestMethod]
        public void StartOnGoRollDoublesOfSixAndNonDoublesOfFourEndsOnTen()
        {
            dieValues = new Queue<Int32>(new[] { 3, 3, 1, 3 });
            turn.Take(horse);

            Assert.AreEqual(10, board.GetPlayerLocation(horse));
        }

        [TestMethod]
        public void PlayerDoesNotRollDoublesMovesRollValues()
        {
            dieValues = new Queue<Int32>(new[] { 3, 1 });
            turn.Take(horse);

            Assert.AreEqual(4, board.GetPlayerLocation(horse));
        }

        [TestMethod]
        public void RollDoublesTwiceMovesThreeRollsTotal()
        {
            dieValues = new Queue<Int32>(new[] { 1, 1, 2, 2, 1, 5 });
            turn.Take(horse);

            Assert.AreEqual(12, board.GetPlayerLocation(horse));
        }

        [TestMethod]
        public void RollDoublesThreeTimesEndOnJustVisiting()
        {
            dieValues = new Queue<Int32>(new[] { 1, 1, 2, 2, 3, 3 });
            turn.Take(horse);

            Assert.AreEqual(ClassicBoardFactory.JustVisitingLocation, board.GetPlayerLocation(horse));
        }

        [TestMethod]
        public void PlayerMortgagesPropertiesAtBeginningOfTurnWhenBalanceIsLessThanTwoHundred()
        {
            var propertiesToMortgage = properties.Take(3);

            foreach (var property in propertiesToMortgage)
                property.Sell(horse);

            var balanceBeforeTurn = banker.GetBalance(horse);
            turn.Begin(horse);

            Assert.IsTrue(propertiesToMortgage.All(p => p.IsMortgaged));
            Assert.AreEqual(balanceBeforeTurn + propertiesToMortgage.Sum(p => p.Price * 9 / 10), banker.GetBalance(horse));
        }

        [TestMethod]
        public void PlayerUnmortgagesPropertiesAtBeginningOfTurnWhenBalanceIsMoreThanTwoHundred()
        {
            banker.Pay(horse, 2000);            

            var property = properties.ElementAt(0);
            property.Sell(horse);
            property.Mortgage();

            var balanceBeforeTurn = banker.GetBalance(horse);
            turn.Begin(horse);

            Assert.IsFalse(property.IsMortgaged);
            Assert.AreEqual(balanceBeforeTurn - property.Price, banker.GetBalance(horse));
        }

        [TestMethod]
        public void PlayerMortgagesPropertiesAtEndOfTurnWhenBalanceIsLessThanTwoHundred()
        {
            var propertiesToMortgage = properties.Take(3);

            foreach (var property in propertiesToMortgage)
                property.Sell(horse);

            var balanceBeforeTurn = banker.GetBalance(horse);
            turn.End(horse);

            Assert.IsTrue(propertiesToMortgage.All(p => p.IsMortgaged));
            Assert.AreEqual(balanceBeforeTurn + propertiesToMortgage.Sum(p => p.Price * 9 / 10), banker.GetBalance(horse));
        }

        [TestMethod]
        public void PlayerUnmortgagesPropertiesAtEndOfTurnWhenBalanceIsMoreThanTwoHundred()
        {
            banker.Pay(horse, 2000);

            var property = properties.ElementAt(0);
            property.Sell(horse);
            property.Mortgage();

            var balanceBeforeTurn = banker.GetBalance(horse);
            turn.End(horse);

            Assert.IsFalse(property.IsMortgaged);
            Assert.AreEqual(balanceBeforeTurn - property.Price, banker.GetBalance(horse));
        }
    }
}
