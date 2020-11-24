using System;
using System.Collections.Generic;
using System.Linq;
using WebApiTest.Models;
using WebApiTest.QueryParametersPaging;
using System.Linq.Dynamic.Core;

namespace WebApiTest.Data
{
    public class SqlWebApiTestRepository : IWebApiTestRepository
    {
        private readonly WebApiTestContext _context;

        public SqlWebApiTestRepository(WebApiTestContext context)
        {
            _context = context;
        }

        public void CreateCommand(Command cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Commands.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            if(cmd == null)
            {
                throw new ArgumentNullException(nameof(cmd));
            }
            _context.Commands.Remove(cmd);
        }

        // public IEnumerable<Command> GetAllCommands(CommandQueryParameters commandQueryParameters)
        // {
        //     //return _context.Commands.ToList();
        //     return _context.Commands.OrderBy(x=>x.HowTo)
        //             .Skip(commandQueryParameters.PageCount * (commandQueryParameters.Page - 1))
        //             .Take(commandQueryParameters.PageCount);
        // }

        public IEnumerable<Command> GetAllCommands(CommandQueryParameters commandQueryParameters)
        {
            IEnumerable<Command> allCommands = _context.Commands
            .OrderBy(commandQueryParameters.OrderBy, commandQueryParameters.Descending);

            if(commandQueryParameters.HasQuery)
            {
                allCommands = allCommands
                .Where(x=>x.HowTo.ToLowerInvariant().Contains(commandQueryParameters.Query.ToLowerInvariant()));
            }


            //return _context.Commands.ToList();
            return allCommands
                    .Skip(commandQueryParameters.PageCount * (commandQueryParameters.Page - 1))
                    .Take(commandQueryParameters.PageCount);
        }

        public int Count()
        {
            return _context.Commands.Count();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(x=>x.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges()>0);
        }

        public void UpdateCommand(Command cmd)
        {
            
        }
    }
}