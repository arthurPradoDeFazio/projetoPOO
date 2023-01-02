using System;
namespace AdaCredit
{
	public class AdaCreditController
	{
		public void RunApplication(string[] args)
		{
			// mostra login
			// loop que pega input, processa
			// depois de processar decide -> ou chama uma outra view, ou chama alguma operacao do modelo
			// se chamou operacao do modelo, possivelmente exibe alguma mensagem e volta pra tela inicial
			var view = new AdaCredit.View();
			_ = view.HomeMenu(args);
		}
	}
}

