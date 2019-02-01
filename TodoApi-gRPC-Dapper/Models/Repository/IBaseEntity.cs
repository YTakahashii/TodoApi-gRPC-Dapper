using System;
namespace TodoApi_gRPC_Dapper.Models.Repository
{
    public interface IBaseEntity
    {
        String Id { get; set; }
    }
}
