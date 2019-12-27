using Microservice.Db;
using Microservice.Logic.Mappers;

namespace Microservice.Logic.Handlers
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
