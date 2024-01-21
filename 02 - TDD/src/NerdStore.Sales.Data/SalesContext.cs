using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Sales.Data.Extensions;

namespace NerdStore.Sales.Data;

public class SalesContext : DbContext, IUnitOfWork
{
    private readonly IMediator _mediator;

    public SalesContext(DbContextOptions<SalesContext> options, IMediator mediator) : base(options)
    {
        _mediator = mediator;
    }

    public async Task<bool> Commit()
    {
        var result = await base.SaveChangesAsync() > 0;

        if (result) await _mediator.PublishEventsAsync(this);

        return result;
    }
}
