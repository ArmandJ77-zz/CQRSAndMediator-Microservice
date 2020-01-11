using MediatR;
using Microservice.Db;

namespace Microservice.Logic.Handlers
{
    public class BaseHandler
    {
        public IMediator Mediator { get; }
        public readonly MicroserviceDbContext _dbContext;

        public BaseHandler(MicroserviceDbContext dbContext, IMediator mediator)
        {
            Mediator = mediator;
            _dbContext = dbContext;
        }
    }
}
