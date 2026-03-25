using Weather.Abstraction.Interfaces.Startup;
using Weather.Model.Dto.Read;
using Weather.Model.Entity;
using Weather.Model.Extensions;
using Weather.Server.Factory.ReadDto.Core;

namespace Weather.Server.Factory.ReadDto
{
    public class BmeReadDtoFactory : ReadDtoFactoryBase<Bme, ReadDtoBme>
    {
    }
}