using System;
namespace AdaCredit
{
	public partial class AdaCreditController
	{
		public void RunApplication(string[] args)
		{
			InitMenus(args);
			while (true)
			{
				menuAtual.Show();
				Console.Clear();
				menuAtual = menuAtual.proximoMenu();
				if (menuAtual == null)
					menuAtual = telaPrincipal;
			}
		}

        
    }
}

