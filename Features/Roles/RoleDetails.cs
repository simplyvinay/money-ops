using System.Threading;
using System.Threading.Tasks;
using MoneyOps.Dto;
using MoneyOps.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MoneyOps.Helpers;
using MoneyOps.Helpers.Extensions;

namespace MoneyOps.Features.Roles
{
    public class RoleDetails
    {
        public class Query : IRequest<object>
        {
            public SingleResourceParameters ResourceParams { get; }

            public Query(
                SingleResourceParameters resourceParams)
            {
                ResourceParams = resourceParams;
            }
        }

        public class QueryHandler : IRequestHandler<Query, object>
        {
            private readonly ApplicationDbContext _dbContext;

            public QueryHandler(
                ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }

            public async Task<object> Handle(
                Query message,
                CancellationToken cancellationToken)
            {
                var role = await _dbContext.Roles.AsNoTracking()
                    .SingleAsync(
                        x => x.Id == message.ResourceParams.Id,
                        cancellationToken);
                var roleDto = new RoleDto(
                    role.Id,
                    role.Name,
                    string.Join(
                        ", ",
                        role.PermissionsInRole));
                return roleDto.ShapeData(message.ResourceParams.Fields);
            }
        }
    }
}