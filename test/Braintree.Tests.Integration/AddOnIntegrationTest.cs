using Braintree.TestUtil;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Braintree.Tests.Integration
{
    [TestFixture]
    public class AddOnIntegrationTest
    {
        private BraintreeGateway gateway;
        private BraintreeService service;

        [SetUp]
        public void Setup()
        {
            gateway = new BraintreeGateway
            {
                Environment = Environment.DEVELOPMENT,
                MerchantId = "integration_merchant_id",
                PublicKey = "integration_public_key",
                PrivateKey = "integration_private_key"
            };

            service = new BraintreeService(gateway.Configuration);
        }

        [Test]
        public void All_ReturnsAllAddOns()
        {
            string addOnId = string.Format("dotnet_add_on{0}", new Random().Next(1000000).ToString());
            service.Post(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 100.00M,
                Description = "a test add-on",
                Id = addOnId,
                Kind = "add_on",
                Name = "add_on_name",
                NeverExpires = false,
                NumberOfBillingCycles = 12
            });

            List<AddOn> collection = gateway.AddOn.All();
            Assert.IsNotEmpty(collection);

            AddOn addOn = collection.Find
            (
                delegate(AddOn a)
                {
                    return a.Id == addOnId;
                }
            );

            Assert.AreEqual(100.00M, addOn.Amount);
            Assert.AreEqual("a test add-on", addOn.Description);
            Assert.AreEqual(addOnId, addOn.Id);
            Assert.AreEqual("add_on", addOn.Kind);
            Assert.AreEqual("add_on_name", addOn.Name);
            Assert.AreEqual(false, addOn.NeverExpires);
            Assert.AreEqual(12, addOn.NumberOfBillingCycles);
            Assert.IsNotNull(addOn.CreatedAt);
            Assert.IsNotNull(addOn.UpdatedAt);
        }

        [Test]
#if netcore
        public async Task AllAsync_ReturnsAllAddOns()
#else
        public void AllAsync_ReturnsAllAddOns()
        {
            Task.Run(async() =>
#endif
        {
            string addOnId = string.Format("dotnet_add_on{0}", new Random().Next(1000000).ToString());
            await service.PostAsync(service.MerchantPath() + "/modifications/create_modification_for_tests", new ModificationRequestForTests {
                Amount = 100.00M,
                Description = "a test add-on",
                Id = addOnId,
                Kind = "add_on",
                Name = "add_on_name",
                NeverExpires = false,
                NumberOfBillingCycles = 12
            });

            List<AddOn> collection = await gateway.AddOn.AllAsync();
            Assert.IsNotEmpty(collection);

            AddOn addOn = collection.Find
            (
                delegate(AddOn a)
                {
                    return a.Id == addOnId;
                }
            );

            Assert.AreEqual(100.00M, addOn.Amount);
            Assert.AreEqual("a test add-on", addOn.Description);
            Assert.AreEqual(addOnId, addOn.Id);
            Assert.AreEqual("add_on", addOn.Kind);
            Assert.AreEqual("add_on_name", addOn.Name);
            Assert.AreEqual(false, addOn.NeverExpires);
            Assert.AreEqual(12, addOn.NumberOfBillingCycles);
            Assert.IsNotNull(addOn.CreatedAt);
            Assert.IsNotNull(addOn.UpdatedAt);
        }
#if net452
            ).GetAwaiter().GetResult();
        }
#endif
    }
}
