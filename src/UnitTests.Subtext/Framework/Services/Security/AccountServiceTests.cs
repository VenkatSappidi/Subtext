﻿using System;
using System.Collections.Specialized;
using System.Web;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Security;

namespace UnitTests.Subtext.Framework.Security
{
    [TestFixture]
    public class AccountServiceTests
    {
        [Test]
        public void Logout_ClearsAuthCookie()
        {
            // arrange
            var responseCookies = new HttpCookieCollection();
            var context = new Mock<ISubtextContext>();
            var existingCookies = new HttpCookieCollection();
            existingCookies.Add(new HttpCookie(".ASPXAUTH.HA.null"));
            context.Setup(c => c.HttpContext.Request.Cookies).Returns(existingCookies);
            context.Setup(c => c.HttpContext.Request.QueryString).Returns(new NameValueCollection());
            context.Setup(c => c.HttpContext.Response.Cookies).Returns(responseCookies);
            var service = new AccountService(context.Object);

            // act
            service.Logout();

            // assert
            Assert.AreEqual(1, responseCookies.Count);
            Assert.IsTrue(responseCookies[0].Expires < DateTime.UtcNow);
        }
    }
}
