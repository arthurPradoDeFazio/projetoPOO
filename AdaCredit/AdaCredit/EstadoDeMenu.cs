using System;
using ConsoleTools;

namespace AdaCredit
{
	public class EstadoDeMenu
	{
		private ConsoleMenu triggerMenu;
		private Dictionary<string, EstadoDeMenu> proximoEstado;

		public EstadoDeMenu(ConsoleMenu triggerMenu)
		{
			this.triggerMenu = triggerMenu;
			proximoEstado = new();
		}

		public void AdicionaEntrada(string opcao, EstadoDeMenu proximoEstado)
		{
			this.proximoEstado.Add(opcao, proximoEstado);
		}

        public EstadoDeMenu? proximoMenu() => proximoEstado.GetValueOrDefault(triggerMenu.CurrentItem.Name);

        public void Show() => triggerMenu.Show();
    }
}

