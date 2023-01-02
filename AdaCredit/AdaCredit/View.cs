using System;
using ConsoleTools;

namespace AdaCredit
{
	public class View
	{
		public int HomeMenu(string[] args)
		{
            var menu = new ConsoleMenu(args, level: 0)
               .Add("One", ConsoleMenu.Close) 
               .Add("Two", ConsoleMenu.Close)
               .Add("Three", ConsoleMenu.Close)
               .Add("Sub", () => { })
               .Add("Change me", () => { })
               .Add("Close", ConsoleMenu.Close)
               .Add("Action then Close", () => { })
               .Add("Exit", () => Environment.Exit(0))

               .Configure(config =>
               {
                   config.Selector = "--> ";
                   config.Title = "Main menu";
               });
            menu.Show();
            menu.CloseMenu();
            Console.WriteLine(menu.CurrentItem.Name);
            return 1;
        }
	}
}

