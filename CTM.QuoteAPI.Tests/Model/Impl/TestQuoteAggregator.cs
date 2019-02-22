using CTM.Contracts;
using CTM.QuoteAPI.Model;
using CTM.QuoteAPI.Model.Impl;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTM.QuoteAPI.Tests.Model.Impl
{
    [TestFixture]
    public class TestQuoteAggregator
    {
        IQuoteAggregator underTest;
        Mock<IQuoteProvider> mockQuoteProvider1;
        Mock<IQuoteProvider> mockQuoteProvider2;

        [SetUp]
        public void SetUp()
        {
            mockQuoteProvider1 = new Mock<IQuoteProvider>();
            mockQuoteProvider2 = new Mock<IQuoteProvider>();
            underTest = new QuoteAggregator(new List<IQuoteProvider>
            {
                mockQuoteProvider1.Object,
                mockQuoteProvider2.Object
            });
        }

        [Test]
        public void TestAllProvidersCalled()
        {
            var req = new QuoteRequest();
            underTest.Aggregate(req);

            mockQuoteProvider1.Verify(s => s.Send(req), Times.Once());
            mockQuoteProvider2.Verify(s => s.Send(req), Times.Once());
        }
    }
}
