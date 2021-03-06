﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HyperLibrary.ResouceClient.Commands;
using HyperLibrary.ResouceClient.Model;

namespace HyperLibrary.ResouceClient
{
    internal class LibraryExplorer
    {
        private readonly IList<ICommand> _defaultCommands; 

        public LibraryExplorer(LibraryApiClient apiClient)
        {
            var bookBag = new BookBag();
            _defaultCommands = new List<ICommand>
            {
                new ExploreLibraryCommand(apiClient,bookBag),
                new ViewCheckedOutBooksCommand(bookBag,apiClient),
                new ExitLibraryCommand(),
            };
        }

        public async Task Explore()
        {
            Console.WriteLine();
            Console.WriteLine("**************** Welcome to the Library ****************");
            await Recurse(new List<ICommand>());
        }

        private async Task Recurse(IEnumerable<ICommand> commands)
        {
            ICommand nextCommand = GetNextCommand(commands);
            if (nextCommand != null && !(nextCommand is ExitLibraryCommand))
            {
                IEnumerable<ICommand> nextOptions = await nextCommand.Execute();
                if (nextOptions.Any())
                {
                    await Recurse(nextOptions);
                }
            }
        }

        private ICommand GetNextCommand(IEnumerable<ICommand> commandOptions)
        {
            Console.WriteLine();
            int optionCount = 0;
            commandOptions = commandOptions!= null ?commandOptions.Concat(_defaultCommands):_defaultCommands;
            foreach (var command in commandOptions)
            {
                Console.WriteLine("{0} - {1}",optionCount,command.Description);
                optionCount++;
            }

            Console.WriteLine();
            Console.WriteLine("What would you like to do?");

            var option = GetIntegerOption(commandOptions.Count());
            return commandOptions.ElementAt(option);
        }

        private static int GetIntegerOption(int maxOption)
        {
            string enteredText = Console.ReadLine();
            try
            {
                int option = Convert.ToInt32(enteredText);
                if (option < 0 || option > maxOption)
                {
                    Console.WriteLine("What was that Captain? Give us any number from 0 to {0}....",maxOption-1);
                    return GetIntegerOption(maxOption);
                }
                return option;
              
            }
            catch (Exception)
            {
                Console.WriteLine("What was that Captain? Give us a number please....");
                return GetIntegerOption(maxOption);
            }    
        }
    }
}