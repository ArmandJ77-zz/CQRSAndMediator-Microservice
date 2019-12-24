using Microservice.Api.Database;
using Microservice.Api.Mappers;

namespace Microservice.Api.Handlers
{
    public class BaseHandler
    {
        public readonly MicroserviceDbContext _dbContext;
        public readonly IMapper _mapper;

        public BaseHandler(MicroserviceDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
