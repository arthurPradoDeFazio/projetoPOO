using System;
using System.Linq;
using Bogus;

namespace AdaCredit
{
	public static class ServicosCliente
	{
		public static void CadastrarCliente()
		{
			Console.Write("Entre com o primeiro nome do cliente: ");
			string? nome = Console.ReadLine();
			if (nome == null)
				throw new IOException("Não foi possível ler o primeiro nome do cliente. Cadastro não realizado"); // TODO criar exceção mais especifica?

			Console.Write("Entre com último nome do cliente: ");
			string? sobrenome = Console.ReadLine();
			if (sobrenome == null)
				throw new IOException("Não foi possível ler o último nome do cliente. Cadastro não realizado");

			Dictionary<string, Cliente> clientes = Cliente.ClientesNoArquivo();
			if (clientes.Any(c => c.Value.Nome == nome && c.Value.Sobrenome == sobrenome))
				throw new ArgumentException("Cliente já cadastrado!");

			string senha = ColetaSenha();
			string conta = GeraNumeroDeConta(clientes);

			var novoCliente = new Cliente {
				Nome = nome,
				Sobrenome = sobrenome,
				Senha = senha,
				Conta = conta,
				DigitoVerficador = Cliente.CalculaDigito(conta),
				Saldo = 0,
				Agencia = "0001",
				Ativo = true
			};

			novoCliente.SalveNoCSV(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Clientes.csv"));

		}

        private class ContaBogus
        {
            public string StringConta { get; set; }
        }

        private static string GeraNumeroDeConta(Dictionary<string, Cliente> clientes)
        {
			Console.WriteLine("Gerando numero de conta");
			
			var geradorDeConta = new Faker<ContaBogus>("pt_BR")
										.RuleFor(s => s.StringConta, f => f.Finance.Account(5));
			Console.WriteLine("Fazendo faker");
			ContaBogus novaConta;
			while (clientes.ContainsKey((novaConta = geradorDeConta.Generate()).StringConta)) { }
			return novaConta.StringConta;
        }

        private static string ColetaSenha()
        {
            string? senha, confirmacao;
				
			Console.Write("Entre com a sua senha: "); // TODO ocultar senha quando for digitar
			senha = Console.ReadLine();

			Console.Write("Entre com a senha novamente para confirmação: "); // TODO ocultar
			confirmacao = Console.ReadLine();
			
			if (senha == null || confirmacao == null)
				throw new IOException("Não foi possível coletar a senha. Cadastro não realizado");

			while (senha != confirmacao)
			{
				Console.Write("Senhas diferentes! Entre novamente com a confirmação: ");
				confirmacao = Console.ReadLine();
				if (confirmacao == null)
					throw new IOException("Não foi possível coletar a senha. Cadastro não realizado");
			}

			return senha;
        }
    }
}

