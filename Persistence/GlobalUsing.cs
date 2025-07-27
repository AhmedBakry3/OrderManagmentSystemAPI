global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Persistence.Data.DbContexts.OrderManagementDbContext;
global using Persistence.Data.DbContexts.StoredIdentityDbContext;
global using StackExchange.Redis;
global using DomainLayer.Contracts;
global using DomainLayer.Models;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Persistence.Data;
global using Persistence.Data.Repositories;
global using Shared.Utilities;
global using AutoMapper;
global using Moq;
global using Shared.DataTransferObject.CustomerModuleDTos;
global using DomainLayer.Exceptions;
global using Xunit;
global using Shared.DataTransferObject.InvoiceModuleDTos;
global using static Service.Specifications.InvoiceModuleSpecifications.InvoiceSpecifications;
global using Service;
global using Shared.DataTransferObject.ProductModuleDTos;
global using Shared.DataTransferObject.IdentityDTos;
global using Demo.Presentation.Helper;
global using ServiceAbstraction;
global using Microsoft.Extensions.Options;
global using Service.Specifications.OrderSpecifications;
global using Shared.DataTransferObject.OrderModuleDTos;
global using Shared.Enum;






