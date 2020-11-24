using System.Collections.Generic;
using WebApiTest.Models;
using WebApiTest.QueryParametersPaging;

namespace WebApiTest.Data
{
    public interface IWebApiTestRepository
    {
        bool SaveChanges();
        IEnumerable<Command> GetAllCommands(CommandQueryParameters commandQueryParameters);
        Command GetCommandById(int id);
        void CreateCommand(Command cmd);
        void UpdateCommand(Command cmd);
        void DeleteCommand(Command cmd);
        int Count();
    }
}