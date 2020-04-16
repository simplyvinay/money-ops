﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MoneyOps.Domain.Identity;
using MoneyOps.Dto;
using MoneyOps.Helpers;
using MoneyOps.Infrastructure.Auth.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MoneyOps.Features.Roles
{
    [Route("api/roles")]
    [HasPermission(Permissions.RoleManager)]
    public class RolesController
    {
        private readonly IMediator _mediator;

        public RolesController(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoleDto>), 200)]
        public async Task<object> Get(
            ListResourceParameters resourceParams)
        {
            return await _mediator.Send(new RoleList.Query(resourceParams));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RoleDto), 200)]
        public async Task<object> Get(
            SingleResourceParameters resourceParams)
        {
            return await _mediator.Send(new RoleDetails.Query(resourceParams));
        }

        [HttpPost]
        public async Task<RoleDto> Create(
            [FromBody] CreateRole.Command command)
        {
            return await _mediator.Send(command);
        }
        
        [HttpPut]
        public async Task<RoleDto> Update(
            [FromBody] UpdateRole.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        public async Task Delete(
            int id)
        {
            await _mediator.Send(new DeleteRole.Command(id));
        }
    }
}