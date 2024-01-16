using NerdStore.Sales.Domain.Enums;
using NerdStore.Sales.Domain.Models;
using NerdStore.Sales.Domain.Validators;

namespace NerdStore.Sales.Domain.Tests;

public class VoucherTests
{
    [Fact(DisplayName = "Voucher Validate Valid Value Type")]
    [Trait("Category", "Sales - Voucher")]
    public void Voucher_VoucherValidateValueType_ShouldBeValid()
    {
        // Arrange
        var voucher = new Voucher(
            code: "PROMO-15-REAIS",
            voucherDiscountType: VoucherDiscountType.Value,
            discountValue: 15,
            discountPercentage: null,
            quantity: 1,
            expirationDate: DateTime.Now,
            active: true,
            isUsed: false);

        // Act
        var result = voucher.ApplyIfApplicable();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Voucher Validate Invalid Value Type")]
    [Trait("Category", "Sales - Voucher")]
    public void Voucher_VoucherValidateValueType_ShouldBeInvalid()
    {
        // Arrange
        var voucher = new Voucher(
            code: "",
            voucherDiscountType: VoucherDiscountType.Value,
            discountValue: null,
            discountPercentage: null,
            quantity: 0,
            expirationDate: DateTime.Now.AddDays(-1),
            active: false,
            isUsed: true);

        // Act
        var result = voucher.ApplyIfApplicable();

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(6, result.Errors.Count);
        Assert.Contains(VoucherApplicableValidator.ActiveErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.CodeErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.ExpirationDateErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.QuantityErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.IsUsedErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.DiscountValueErrorMessage, result.Errors.Select(c => c.ErrorMessage));
    }

    [Fact(DisplayName = "Voucher Validate Valid Percentage Type")]
    [Trait("Category", "Sales - Voucher")]
    public void Voucher_VoucherValidatePercentageType_ShouldBeValid()
    {
        // Arrange
        var voucher = new Voucher(
            code: "PROMO-15-REAIS",
            voucherDiscountType: VoucherDiscountType.Percentage,
            discountValue: null,
            discountPercentage: 15,
            quantity: 1,
            expirationDate: DateTime.Now,
            active: true,
            isUsed: false);

        // Act
        var result = voucher.ApplyIfApplicable();

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact(DisplayName = "Voucher Validate Invalid Percentage Type")]
    [Trait("Category", "Sales - Voucher")]
    public void Voucher_VoucherValidatePercentageType_ShouldBeInvalid()
    {
        // Arrange
        var voucher = new Voucher(
            code: "",
            voucherDiscountType: VoucherDiscountType.Percentage,
            discountValue: null,
            discountPercentage: null,
            quantity: 0,
            expirationDate: DateTime.Now.AddDays(-1),
            active: false,
            isUsed: true);

        // Act
        var result = voucher.ApplyIfApplicable();

        // Assert
        Assert.False(result.IsValid);
        Assert.Equal(6, result.Errors.Count);
        Assert.Contains(VoucherApplicableValidator.ActiveErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.CodeErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.ExpirationDateErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.QuantityErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.IsUsedErrorMessage, result.Errors.Select(c => c.ErrorMessage));
        Assert.Contains(VoucherApplicableValidator.DiscountPercentageErrorMessage, result.Errors.Select(c => c.ErrorMessage));
    }
}