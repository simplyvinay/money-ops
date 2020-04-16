﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MoneyOps.Dto;
using MoneyOps.Helpers;
using MoneyOps.Helpers.Extensions;
using MoneyOps.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;

namespace MoneyOps.Features.Users
{
    public class UserList
    {
        public class Query : IRequest<object>
        {
            public ListResourceParameters ResourceParams { get; }

            public Query(
                ListResourceParameters resourceParams)
            {
                ResourceParams = resourceParams;
            }
        }

        public class QueryHandler : IRequestHandler<Query, object>
        {
            private readonly ApplicationDbContext _dbContext;
            private readonly ISieveProcessor _sieveProcessor;

            public QueryHandler(
                ApplicationDbContext dbContext,
                ISieveProcessor sieveProcessor)
            {
                _dbContext = dbContext;
                _sieveProcessor = sieveProcessor;
            }

            //Implement Paging
            public async Task<object> Handle(
                Query message,
                CancellationToken cancellationToken)
            {
                var users = _dbContext.Users.AsNoTracking()
                    .Include(x => x.Roles)
                    .ThenInclude(x => x.Role);

                //var count = _sieveProcessor.Apply(message.ResourceParams, users, applyPagination: false).Count();

                var filteredUsers = await _sieveProcessor.Apply(
                    message.ResourceParams,
                    users).ToListAsync(cancellationToken);

                var userDetailsDtos = filteredUsers
                    .Select(
                        user => new UserDetailsDto(
                            user.Id,
                            user.FirstName,
                            user.LastName,
                            user.FullName,
                            user.Email,
                            string.Join(
                                ", ",
                                user.Roles.Select(r => r.Role.Name)))
                    ).ToList();
                return userDetailsDtos.ShapeData(message.ResourceParams.Fields);
            }
        }
    }
}