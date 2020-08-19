using FluentAssertions;
using Shopping_Cart.Domains.Catalog.Concrete;
using Shopping_Cart.Domains.Discount.Concrete;
using Shopping_Cart.Domains.ShoppingCart.Concrete;
using System;
using Xunit;

namespace Shopping_Cart_XUnitTest
{
    public class ShoppingCartTest
    {

        [Fact]
        public void ShoppingCartItemCountShouldBeOne()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            cart.AddItem(new Product("Ayakkab�", 100, new Category("Ayakkab�lar")), 1);
            cart.ShoppingCartItems.Count.Should().Be(1);
        }

        [Fact]
        public void CouponShouldNotBeApplied()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Coupon coupon = new Coupon(150, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate, 10);
            cart.ApplyCoupon(coupon);
            cart.AppliedCoupons.Count.Should().Be(0);
        }

        [Fact]
        public void CouponShouldBeApplied()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            cart.AddItem(new Product("Nike Ayakkab�", 300, new Category("Nike Ayakkab�lar")), 2);
            Coupon coupon = new Coupon(150, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate, 10);
            cart.ApplyCoupon(coupon);
            cart.AppliedCoupons.Count.Should().Be(1);
        }

        [Fact]
        public void CampaignShouldNotBeApplied()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Campaign campaign = new Campaign(new Category("Ayakkab�"), 10, 5, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate);
            cart.ApplyDiscounts(campaign);
            cart.AppliedCampaigns.Count.Should().Be(0);
        }

        [Fact]
        public void CampaignShouldBeApplied()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            cart.AddItem(new Product("Nike Ayakkab�", 300, category), 2);

            Campaign campaign = new Campaign(category, 10, 1, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate);
            cart.ApplyDiscounts(campaign);
            cart.AppliedCampaigns.Count.Should().Be(1);
        }

        [Fact]
        public void CampaignAndCouponShouldBeApplied()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            cart.AddItem(new Product("Nike Ayakkab� - 1", 100, category), 1);
            cart.AddItem(new Product("Nike Ayakkab� - 2", 200, category), 2);
            cart.AddItem(new Product("Nike Ayakkab� - 3", 300, category), 3);

            Campaign campaign = new Campaign(category, 10, 3, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate);
            Coupon coupon = new Coupon(600, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate, 10);
            cart.ApplyDiscounts(campaign);
            cart.ApplyCoupon(coupon);

            cart.AppliedCampaigns.Count.Should().Be(1);
            cart.AppliedCoupons.Count.Should().Be(1);
        }

        [Fact]
        public void GetCouponDiscountShouldBeEqualTen()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            cart.AddItem(new Product("Nike Ayakkab� - 1", 100, category), 1);
            Coupon coupon = new Coupon(100, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate, 10);
            cart.ApplyCoupon(coupon);
            cart.GetCouponDiscounts().Should().Be(10);
        }

        [Fact]
        public void GetCampaignDiscountShouldBeEqualFifty()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            Campaign campaign = new Campaign(category, 50, 1, Shopping_Cart.Domains.Discount.Enums.DiscountType.Rate);

            cart.AddItem(new Product("Nike Ayakkab� - 1", 100, category), 1);

            cart.ApplyDiscounts(campaign);
            cart.GetCampaignDiscounts().Should().Be(50);
        }

        [Fact]
        public void CampaignAndCouponDiscountShouldBeEqualFifty()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            Campaign campaign = new Campaign(category, 50, 1, Shopping_Cart.Domains.Discount.Enums.DiscountType.Amount);
            Coupon coupon = new Coupon(100, Shopping_Cart.Domains.Discount.Enums.DiscountType.Amount, 50);
            cart.AddItem(new Product("Nike Ayakkab� - 1", 150, category), 1);
            cart.ApplyDiscounts(campaign);
            cart.ApplyCoupon(coupon);
            cart.GetCampaignDiscounts().Should().Be(50);
            cart.GetCouponDiscounts().Should().Be(50);
        }

        [Fact]
        public void DeliveryCostShouldBeTwoPointNintyNine()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            cart.GetDeliveryCost().Should().Be(2.99);
        }

        [Fact]
        public void DeliveryCostShouldBeFivePointNintyNine()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            Product p_1 = new Product("Nike 1", 10, category);
            Product p_2 = new Product("Nike 2", 10, category);

            cart.AddItem(p_1, 2);
            cart.AddItem(p_2, 2);
            cart.GetDeliveryCost().Should().Be(5.99);
        }
        [Fact]
        public void TotalAmountAfterDiscountShouldBeThreeHundred()
        {
            ShoppingCart cart = new ShoppingCart(1, 1);
            Category category = new Category("Nike Ayakkab�lar");
            Product p_1 = new Product("Nike 1", 200, category);
            Product p_2 = new Product("Nike 2", 200, category);
            Coupon coupon = new Coupon(400, Shopping_Cart.Domains.Discount.Enums.DiscountType.Amount, 100);
            cart.AddItem(p_1, 1);
            cart.AddItem(p_2, 1);
            cart.ApplyCoupon(coupon);
            cart.GetTotalAmountAfterDiscounts().Should().Be(300);
        }

    }
}

