<h2>Project Folder Structure</h2>

ECommerceProject.sln
- Bootstrapper
  [Program.cs]
- Modules
    - Customer
        - ECommerceProject.Modules.Customers.Api.csproj
            - Controllers
            - [...]
        - ECommerceProject.Modules.Customers.Application.csproj
            - Commands
            - Exceptions
            - Interfaces
            - Models
        - ECommerceProject.Modules.Customers.Domain.csproj
            - Entities
            - Events
            - ValueObjects
        - ECommerceProject.Modules.Customers.Infrastructure.csproj
            - Extensions
            - Identity
            - Persistence
            - Services
              [EmailService.cs]
    - Orders
        - [benzer şekilde]
- Shared
    - ECommerceProject.Shared.Abstractions.csproj
        - Commands
          [ICommand.cs, ICommandHandler.cs]
        - Domain
          [AggregateRoot.cs, ValueObject.cs]
        - Events
          [IDomainEvent.cs]
        - Exceptions
          [SharedExcepiton.cs]
        - Queries
          [IQuery.cs, IQueryHandler.cs]
        - Repository
          [IRepository.cs, IUnitOfWork.cs]
    - ECommerceProject.Shared.Infrastructure.csproj
        - Auth
        - Caching
          [CacheService.cs]
        - Database
          [DbContextBase.cs, UnitOfWork.cs]
        - Repository
          [BaseRepository.cs]



<h2>Technologies:</h2>
- DDD
- Modular Monolith
- CQRS (mediatr)
- EF Core (postgresql)
- Repository, Unit of work
- Asp.net core Identity (jwt authentication)
- Redis Cache