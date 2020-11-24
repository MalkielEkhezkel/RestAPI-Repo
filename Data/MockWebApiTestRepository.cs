using System.Collections.Generic;
using WebApiTest.Models;
using WebApiTest.QueryParametersPaging;

namespace WebApiTest.Data
{
    public class MockWebApiTestRepository : IWebApiTestRepository
    {
        public int Count()
        {
            throw new System.NotImplementedException();
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Command> GetAllCommands(CommandQueryParameters commandQueryParameters)
        {
            var commands = new List<Command>
            {
                new Command{Id = 0, HowTo = "Go go", Line = "Line 1", Platform = "Platform Name"},
                new Command{Id = 1, HowTo = "Hey Hey", Line = "Line 2", Platform = "Platform Name 2"},
                new Command{Id = 2, HowTo = "Ku Ku", Line = "Line 3", Platform = "Platform Name 3"}
            };

            return commands;
        }

        public Command GetCommandById(int id)
        {
            return new Command{Id = 0, HowTo = "Go go", Line = "Fight", Platform = "Platform Name"};
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}