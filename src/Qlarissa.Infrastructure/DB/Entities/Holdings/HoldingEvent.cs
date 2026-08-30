using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Qlarissa.Infrastructure.DB.Entities.Holdings;

public class HoldingEvent
{
    public int Id { get; set; }
    public int HoldingId { get; set; }
    public HoldingEventType EventType { get; set; }
    public int Quantity { get; set; }
    public DateOnly Date { get; set; }
}

public enum HoldingEventType
{
    Buy,
    Sell,
}

public class HoldingEventConfiguration : IEntityTypeConfiguration<HoldingEvent>
{
    public void Configure(EntityTypeBuilder<HoldingEvent> builder)
    {
        builder.Property(holdingEvent => holdingEvent.Id).ValueGeneratedOnAdd();

        builder.Property(holdingEvent => holdingEvent.Date)
                .HasConversion(d => d.ToDateTime(TimeOnly.MinValue),
                           d => DateOnly.FromDateTime(d));
    }
}