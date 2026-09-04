namespace Qlarissa.WebAPI.Models.Security.Base;

public abstract class SecurityBase
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ShortName { get; set; } = string.Empty;

    public Currency Currency { get; set; }

    public SecurityType SecurityType { get; set; }

    public static void FromDomainEntity(Domain.Entities.Securities.Base.SecurityBase domainEntity, SecurityBase webApiModel)
    {
        webApiModel.Id = domainEntity.Id;
        webApiModel.Name = domainEntity.Name;
        webApiModel.ShortName = domainEntity.ShortName;
        webApiModel.Currency = Currency.FromDomainEntity(domainEntity.Currency);
        webApiModel.SecurityType = (SecurityType)domainEntity.SecurityType;
    }
}