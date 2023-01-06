using System;
using System.Globalization;
using System.Linq;
using Bogus;
using CsvHelper.Configuration;

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

		public static void ConsultaDados()
        {
            Cliente clienteConsultado = ColetaCliente();
            Console.WriteLine($"Informações do cliente:{Environment.NewLine}");
            Console.WriteLine(clienteConsultado);
        }

        private static Cliente ColetaCliente()
        {
            Console.Write("Entre com o número da conta: ");
            string? conta = Console.ReadLine();
            if (conta == null)
                throw new IOException("Não foi possível ler o número da conta");

            var clientes = Cliente.ClientesNoArquivo();
            if (!clientes.TryGetValue(conta, out Cliente? clienteConsultado))
                throw new IOException("Não há cliente com esse número de conta!");

            Console.Write("Entre com o dígito verificador da conta: ");
            char? digito = Console.ReadKey().KeyChar;
            if (digito == null)
                throw new IOException("Não foi possível ler o primeiro nome do cliente. Cadastro não realizado");
            if (digito != Cliente.CalculaDigito(conta))
                throw new ArgumentException("Dígito verificador incorreto");

            Console.Write("Entre com a senha: ");
            string? senha = Console.ReadLine();
            if (senha == null)
                throw new IOException("Não foi possível ler a senha");
			if (senha != clienteConsultado.Senha)
				throw new ArgumentException("Senha incorreta!");

			return clienteConsultado;
        }

		public static void DesativarCadastro()
		{
			var cliente = ColetaCliente();
			if (!cliente.Ativo)
				return;

			var novoCliente = new Cliente
			{
				Nome = cliente.Nome,
				Sobrenome = cliente.Sobrenome,
				Saldo = cliente.Saldo,
				Conta = cliente.Conta,
				DigitoVerficador = cliente.DigitoVerficador,
				Senha = cliente.Senha,
				Agencia = cliente.Agencia,
				Ativo = false
			};

			var clientes = Cliente.ClientesNoArquivo();
			clientes[novoCliente.Conta] = novoCliente;
			SubstituaArquivo(clientes);
		}

		public static void AlteraNome() // dá pra usar Template Method pattern com alterar sobrenome e senha -> refatorar
		{
            var cliente = ColetaCliente();
            if (!cliente.Ativo)
                throw new ArgumentException("Cliente inativo!");

            Console.Write("Entre com o novo nome: ");
            string? novoNome = Console.ReadLine();
            if (novoNome == null)
                throw new IOException("Não foi possível ler o novo nome");
            var novoCliente = new Cliente
            {
                Nome = novoNome,
                Sobrenome = cliente.Sobrenome,
                Saldo = cliente.Saldo,
                Conta = cliente.Conta,
                DigitoVerficador = cliente.DigitoVerficador,
                Senha = cliente.Senha,
                Agencia = cliente.Agencia,
                Ativo = cliente.Ativo
            };

            var clientes = Cliente.ClientesNoArquivo();
            clientes[novoCliente.Conta] = novoCliente;
            ServicosCliente.SubstituaArquivo(clientes);
        }

        public static void AlteraSobrenome()
        {
            var cliente = ColetaCliente();
            if (!cliente.Ativo)
                throw new ArgumentException("Cliente inativo!");

            Console.Write("Entre com o novo sobrenome: ");
            string? novoSobrenome = Console.ReadLine();
            if (novoSobrenome== null)
                throw new IOException("Não foi possível ler o novo sobrenome");
            var novoCliente = new Cliente
            {
                Nome = cliente.Nome,
                Sobrenome = novoSobrenome,
                Saldo = cliente.Saldo,
                Conta = cliente.Conta,
                DigitoVerficador = cliente.DigitoVerficador,
                Senha = cliente.Senha,
                Agencia = cliente.Agencia,
                Ativo = cliente.Ativo
            };

            var clientes = Cliente.ClientesNoArquivo();
            clientes[novoCliente.Conta] = novoCliente;
            ServicosCliente.SubstituaArquivo(clientes);
        }

        public static void AlteraSenha()
        {
            var cliente = ColetaCliente();
            if (!cliente.Ativo)
                throw new ArgumentException("Cliente inativo!");

            Console.Write("Entre com a nova senha: ");
            string? novaSenha = Console.ReadLine();
            if (novaSenha== null)
                throw new IOException("Não foi possível ler a nova senha");
            var novoCliente = new Cliente
            {
                Nome = cliente.Nome,
                Sobrenome = cliente.Sobrenome,
                Saldo = cliente.Saldo,
                Conta = cliente.Conta,
                DigitoVerficador = cliente.DigitoVerficador,
                Senha = novaSenha,
                Agencia = cliente.Agencia,
                Ativo = cliente.Ativo
            };

            var clientes = Cliente.ClientesNoArquivo();
            clientes[novoCliente.Conta] = novoCliente;
            SubstituaArquivo(clientes);
        }

        private static void SubstituaArquivo(Dictionary<string, Cliente> clientes)
        {
			var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";"
            };
            string nomeDoArquivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Clientes.csv");

            using (var leitor = new StreamWriter(nomeDoArquivo))
            using (var csv = new CsvHelper.CsvWriter(leitor, config))
            {
                csv.Context.RegisterClassMap<Cliente.ClienteMap>();
				csv.WriteRecords(clientes.Select(entrada => entrada.Value));
            }
		}

        private class ContaBogus
        {
            public string StringConta { get; set; }
        }

        private static string GeraNumeroDeConta(Dictionary<string, Cliente> clientes)
        {			
			var geradorDeConta = new Faker<ContaBogus>("pt_BR")
										.RuleFor(s => s.StringConta, f => f.Finance.Account(5));
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

